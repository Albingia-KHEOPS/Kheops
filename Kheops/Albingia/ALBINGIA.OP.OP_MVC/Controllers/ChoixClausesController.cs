using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Filters;
using ALBINGIA.OP.OP_MVC.Models;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using ALBINGIA.OP.OP_MVC.Models.ModelesObjet;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;

using EmitMapper;
using Mapster;
using OP.WSAS400.DTO;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.ChoixClauses;
using OP.WSAS400.DTO.Clause;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Regularisation;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using Domain = Albingia.Kheops.OP.Domain;
namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class ChoixClausesController : RemiseEnVigueurController<ModeleChoixClausesPage> {
        private const string contexteOrigine = "Tous";
        private string ModeNAVIG = "modeNavig";
        private string leFiltre = string.Empty;

        public ChoixClausesController() : base() { }

        public override bool IsReadonly => base.IsReadonly || Model.IsReadOnly;

        /// <summary>
        /// Indexes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult Index(string id) // id = CodeOffre_Version_provenance, provenance: '/' = '¤' et '_' = '£'
        {
            string idBeforFormat = id;
            id = InitializeParams(id);

            // CR565 List Clauses (Formule et conditions tarifaires de garantie) mutualisation
            if (idBeforFormat.Contains(AlbConstantesMetiers.TYPE_CLAUSE_PROVENANCE_FORMULE)) {
                //WAT ?
                string dataInfo = ParseToJsonDataInfo(id);
                string dataIntermediaire = ParseToJsonDataIntermediaire("OPT");
                string codeRisque = (string.IsNullOrEmpty(model.CodeRisque)) ? string.Empty : model.CodeRisque;
                string codeRisqueObj = (string.IsNullOrEmpty(model.RisqueObj)) ? "0" : model.RisqueObj;

                return Suivant(dataInfo: dataInfo, dataIntermediaire: dataIntermediaire, perimetre: "OPT", risque: codeRisque, objet: codeRisqueObj, modeNavig: model.ModeNavig, isModeConsultationEcran: model.IsReadOnly, isForceReadOnly: model.IsForceReadOnly);
            }

            Model.IsModifHorsAvenant = IsModifHorsAvenant;

            LoadInfoPage(id);

            // CR565 List Clauses (Formule et conditions tarifaires de garantie) mutualisation
            if (idBeforFormat.Contains(AlbConstantesMetiers.TYPE_CLAUSE_PROVENANCE_CONDITION)) {
                model.ChoixClauseIntermediaire.Etapes = model.ChoixClauseIntermediaire.Etapes.Where(it => it.Value.IsIn("GAR", "OPT")).ToList();
                model.ChoixClauseIntermediaire.Etape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie);
            }

            if (this.model.AllParameters.Folder.Type == AlbConstantesMetiers.TYPE_CONTRAT) {
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
            }
            else {
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }

            if (model.ActeGestion.Equals("REGUL") && model.ActeGestionRegule.Equals("REGUL") && model.Context != null) {
                if (model.Context.Mode == RegularisationMode.Standard) {
                    bool isSimplifiedRegule = false;
                    string motifRegul = null;
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRegularisation>()) {
                        isSimplifiedRegule = client.Channel.IsSimplifiedReguleFlow(model.Context);
                        motifRegul = client.Channel.GetMotifRegularisation(model.RgId);
                    }

                    if (isSimplifiedRegule && motifRegul == "INFERIEURE") {
                        //WAT ??
                        string dataInfo = ParseToJsonDataInfo(id);
                        string dataIntermediaire = ParseToJsonDataIntermediaire("REGUL");
                        string codeRisque = (string.IsNullOrEmpty(model.CodeRisque)) ? string.Empty : model.CodeRisque;
                        string codeRisqueObj = (string.IsNullOrEmpty(model.RisqueObj)) ? "0" : model.RisqueObj;

                        return Suivant(dataInfo: dataInfo, dataIntermediaire: dataIntermediaire, perimetre: "REGUL", risque: codeRisque, objet: codeRisqueObj, modeNavig: model.ModeNavig, isModeConsultationEcran: model.IsReadOnly, isForceReadOnly: model.IsForceReadOnly);
                    }
                }
            }

            Model.Bandeau = null;
            SetBandeauNavigation(id.Split('_').Skip(2).FirstOrDefault());
            SetArbreNavigation();

            return View(model);
        }
        /// <summary>
        /// Enregistre si necessaire, redirige sur la page des risques
        /// </summary>
        /// <param name="dataInfo"></param>
        /// <param name="dataIntermediaire"></param>
        /// <param name="modeNavig"></param>
        /// <returns></returns>
        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Suivant(string dataInfo, string dataIntermediaire, string perimetre, string risque, string objet, string modeNavig, bool isModeConsultationEcran, bool isForceReadOnly) {
            var serialiserInfo = AlbJsExtendConverter<ChoixClauses_Index_MetaModel>.GetSerializer();
            var listClauses = serialiserInfo.ConvertToType<ChoixClauses_Index_MetaModel>(serialiserInfo.DeserializeObject(dataInfo));
            string id =  listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + "_" + listClauses.CodeFormule + "_" + "1" +
                     GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue + ((isModeConsultationEcran || isForceReadOnly) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1")) + GetFormatModeNavig(listClauses.ModeNavig),
                     returnHome = listClauses.txtSaveCancel,
                     guidTab = listClauses.TabGuid;
            var affaire = new Models.Affaire(id);
            var affid = affaire.Adapt<Domain.Affaire.AffaireId>();
            List <Albingia.Kheops.DTO.FormuleDto> formuleList;
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormulePort>())
            {
                formuleList = client.Channel.GetFormulesAffaire(affid, model.IsReadOnly).Formules.ToList();
            }
            listClauses.ModeNavig = modeNavig;
            var modeleIntermediaire = serialiserInfo.ConvertToType<ModeleChoixClause>(serialiserInfo.DeserializeObject(dataIntermediaire));
            listClauses.ChoixClauseIntermediaire = modeleIntermediaire;
            int versionOffre = -1;
            var typeDossier = listClauses.Type.ToLower() == "o" ? "de l'offre : " : "du contrat :";
            if (!int.TryParse(listClauses.Version, out versionOffre)) {

                throw new AlbFoncException(string.Format("Erreur de version {0} {1}", typeDossier, listClauses.CodeOffre), trace: true);
            }
            if (listClauses.ChoixClauseIntermediaire == null) {
                throw new AlbTechException(
                    new Exception(
                        string.Format(
                            "Erreur {0} {1}: Objet ChoixClauseIntermediaire est null- Controlleur :ChoiClausesController",
                            typeDossier, listClauses.CodeOffre)));
            }
            var numAvn = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNID);

            var acteGestion = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNTYPE);
            var isOffreReadonly = GetIsReadOnly(listClauses.TabGuid, listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type, numAvn);

            if (!isOffreReadonly && modeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Standard) {
                //******* Controles de compatibilités des clauses
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var wsGarRsq = channelClient.Channel;
                    int formule = 0;
                    int option = 0;

                    if (listClauses.ChoixClauseIntermediaire.Etape ==
                        AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option)) {

                        if (!int.TryParse(listClauses.CodeFormule, out formule)) {

                            throw new AlbFoncException(
                                string.Format("Erreur numéro de formule {0} {1}", typeDossier, listClauses.CodeOffre),
                                trace: true);
                        }

                        if (!int.TryParse(listClauses.CodeOption, out option)) {

                            throw new AlbFoncException(
                                string.Format("Erreur numéro d'option {0} {1}", typeDossier, listClauses.CodeOffre),
                                trace: true);
                        }
                    }

                    if (GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNTYPE) == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation)) {
                        risque = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.ATTESTID);
                    }

                    if (listClauses.Provenance.Contains(AlbConstantesMetiers.TYPE_CLAUSE_PROVENANCE_CONDITION)) {
                        if (!int.TryParse(listClauses.CodeFormule, out formule)) {

                            throw new AlbFoncException(
                                string.Format("Erreur numéro de formule {0} {1}", typeDossier, listClauses.CodeOffre),
                                trace: true);
                        }

                        if (!int.TryParse(listClauses.CodeOption, out option)) {

                            throw new AlbFoncException(
                                string.Format("Erreur numéro d'option {0} {1}", typeDossier, listClauses.CodeOffre),
                                trace: true);
                        }

                        string resultControlesClauses = wsGarRsq.VerifierContraintesClauses(listClauses.CodeOffre, versionOffre.ToString(), listClauses.Type,
                                                                                        "OPT", "**", "OPT", risque, objet, formule.ToString(), option.ToString(), "ANEXINDISP");
                        if (!string.IsNullOrEmpty(resultControlesClauses)) {
                            throw new AlbFoncException(
                                   resultControlesClauses,
                                   trace: true);
                        }

                        resultControlesClauses = wsGarRsq.VerifierContraintesClauses(listClauses.CodeOffre, versionOffre.ToString(), listClauses.Type,
                                                                                        "GAR", "**", "GAR", risque, objet, formule.ToString(), option.ToString(), "ANEXINDISP");
                        if (!string.IsNullOrEmpty(resultControlesClauses)) {
                            throw new AlbFoncException(
                                   resultControlesClauses,
                                   trace: true);
                        }
                    }
                    else {
                        //SLA : réactivation sur demande de DAN (01.04.2015)
                        string resultControlesClauses = wsGarRsq.VerifierContraintesClauses(listClauses.CodeOffre, versionOffre.ToString(), listClauses.Type,
                                                                                        perimetre, "**", listClauses.ChoixClauseIntermediaire.Etape, risque, objet, formule.ToString(), option.ToString(), "ANEXINDISP");
                        if (!string.IsNullOrEmpty(resultControlesClauses)) {
                            throw new AlbFoncException(
                                   resultControlesClauses,
                                   trace: true);
                        }
                    }
                }
            }

            if (listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option))
                return SuivantGaranties(listClauses, isForceReadOnly);
            if (listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie))
                return SuivantConditions(listClauses, isModeConsultationEcran, isForceReadOnly);
            if (listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin)
                || listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Resiliation)
                || listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.RemiseEnVigueur)) {
                return SuivantFinOffre(listClauses);
            }
            if (listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation)) {
                return SuivantAttestation(listClauses);
            }
            if (listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule)) {
                return SuivantFinregul(listClauses);
                //return OpenAvnModif(listClauses);
            }
            if (listClauses.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque) && formuleList.Find(f => (f.Risque != null ? f.Risque.Numero.ToString() : "0") == risque) != null)
            {
                return SuivantFormules(listClauses, formuleList.Find(f => (f.Risque != null ? f.Risque.Numero.ToString() : "0") == risque).FormuleNumber, isModeConsultationEcran, isForceReadOnly);
            }

                return SuivantDefault(listClauses, isModeConsultationEcran, isForceReadOnly);
        }
        
      
        private RedirectToRouteResult SuivantFormules(ChoixClauses_Index_MetaModel listClauses,int Codeformule, bool isModeConsultationEcran, bool isForceReadOnly)
        
            {
             if (!string.IsNullOrEmpty(listClauses.txtParamRedirect))
                {
                    var tabParamRedirect = listClauses.txtParamRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                var numAvn = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNID);
                var isOffreReadonly = GetIsReadOnly(listClauses.TabGuid, listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type, numAvn);
            return RedirectToAction("Index", "FormuleGarantie", new
          
                 {
                     id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + "_" + Codeformule + "_" + "1" +
                     GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue + ((isModeConsultationEcran || isOffreReadonly) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1")) + GetFormatModeNavig(listClauses.ModeNavig),
                     returnHome = listClauses.txtSaveCancel,
                     guidTab = listClauses.TabGuid
                 });
           
            }
        
        /// <summary>
        /// Supprimes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        ///
        [ErrorHandler]
        public ActionResult Supprime(
            string id,
            string codeOffreContrat,
            int versionOffreContrat,
            string typeOffreContrat,
            int codeAvn,
            string etape,
            string provenance,
            bool fullScreen,
            string tabGuid,
            string codeRisque,
            string codeFormule,
            string codeOption,
            string filtre,
            string modeNavig,
            string acteGestion,
            string acteGestionRegule,
            string colTri,
            string imgTri
            ) {
            if (modeNavig.ParseCode<ModeConsultation>() != ModeConsultation.Standard) {
                throw new Exception("Operation interdite en historique");
            }
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;

                serviceContext.SupprimeClauseUnique(id);
            }
            return this.Filtrer(
                codeOffreContrat: codeOffreContrat,
                versionOffreContrat: versionOffreContrat,
                typeOffreContrat: typeOffreContrat,
                codeAvn: codeAvn,
                etape: etape,
                provenance: provenance,
                fullScreen: fullScreen,
                tabGuid: tabGuid,
                codeRisque: codeRisque,
                codeFormule: codeFormule,
                codeOption: codeOption,
                filtre: filtre,
                modeNavig: modeNavig,
                acteGestion: acteGestion,
                acteGestionRegule: acteGestionRegule,
                false, false,
                colTri: colTri,
                imgTri: imgTri
            );
        }

        
        [ErrorHandler]
        public ActionResult Filtrer(string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string etape, string provenance, bool fullScreen,
            string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string acteGestion, string acteGestionRegule, 
            bool isReadonly, bool isModifHorsAvn,
            string colTri = null, string imgTri = null) {
            if (colTri == null) {
                colTri = "Contexte";
            }

            ModeleChoixClause toReturn = GetInfoChoixClauses(
                colTri,
                codeOffreContrat,
                versionOffreContrat,
                typeOffreContrat,
                codeAvn,
                etape,
                provenance,
                tabGuid,
                codeRisque,
                codeFormule,
                codeOption,
                filtre,
                modeNavig,
                acteGestion,
                acteGestionRegule,
                imgTri: imgTri
                );

            toReturn.Contextes = GetContextes(toReturn.TableauClauses);
            toReturn.Provenance = provenance;
            toReturn.FullScreen = fullScreen;
            toReturn.Filtres = GetFiltres(filtre);
            toReturn.Filtre = filtre;
            SetLayoutInfoTabModel(codeOffreContrat, versionOffreContrat, typeOffreContrat, tabGuid, toReturn);

            #region CR565 CR565 List Clauses (Formule et conditions tarifaires de garantie) mutualisation
            if (toReturn.Provenance.Contains(AlbConstantesMetiers.TYPE_CLAUSE_PROVENANCE_CONDITION)) {
                toReturn.Etapes = toReturn.Etapes.Where(it => it.Value.Equals("GAR") || it.Value.Equals("OPT")).ToList();
            }
            #endregion

            toReturn.IsReadOnly = isReadonly;
            toReturn.IsModifHorsAvenant = isModifHorsAvn;

            toReturn.TableauClauses.ForEach(el => el.IsReadOnlyMode = isReadonly || isModifHorsAvn);
            toReturn.TableauClauses.ForEach(el => el.IsModifHorsAvenant = isModifHorsAvn);

            return PartialView("ChoixClauseIntermediaire", toReturn);
        }



        private static void SetLayoutInfoTabModel(string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, string tabGuid, MetaModelsBase toReturn) {
            string id = codeOffreContrat.Trim() + "_" + versionOffreContrat + "_" + typeOffreContrat + tabGuid;
            if (typeOffreContrat == "P") {
                var contratDto = new ContratDto {
                    CodeContrat = codeOffreContrat,
                    VersionContrat = long.Parse(versionOffreContrat.ToString()),
                    Type = typeOffreContrat
                };
                toReturn.Contrat = contratDto;
            }
            else if (typeOffreContrat == "O") {
                var offreDto = new OffreDto {
                    CodeOffre = codeOffreContrat,
                    Version = int.Parse(versionOffreContrat.ToString()),
                    Type = typeOffreContrat
                };
                toReturn.Offre = new Offre_MetaModel {
                    CodeOffre = codeOffreContrat,
                    Version = int.Parse(versionOffreContrat.ToString()),
                    Type = typeOffreContrat
                };
            }
            SetGuid(toReturn, id, out id);
        }

        private static List<AlbSelectListItem> GetFiltres(string paramSelected = "") {
            var filtres = new List<AlbSelectListItem>();
            filtres.Insert(0, new AlbSelectListItem { Value = AlbConstantesMetiers.Toutes, Text = "Toutes", Selected = paramSelected == AlbConstantesMetiers.Toutes, Title = "Toutes" });
            filtres.Insert(1, new AlbSelectListItem { Value = AlbConstantesMetiers.ToutesSaufObligatoires, Text = "Toutes sauf obligatoires", Selected = string.IsNullOrEmpty(paramSelected) || paramSelected == AlbConstantesMetiers.ToutesSaufObligatoires, Title = "Toutes sauf obligatoires" });
            filtres.Insert(2, new AlbSelectListItem { Value = AlbConstantesMetiers.Suggerees, Text = "Suggérées", Selected = paramSelected == AlbConstantesMetiers.Suggerees, Title = "Suggérées" });
            filtres.Insert(3, new AlbSelectListItem { Value = AlbConstantesMetiers.Ajoutees, Text = "Ajoutées", Selected = paramSelected == AlbConstantesMetiers.Ajoutees, Title = "Ajoutées" });
            filtres.Insert(4, new AlbSelectListItem { Value = AlbConstantesMetiers.Obligatoires, Text = "Obligatoires", Selected = paramSelected == AlbConstantesMetiers.Obligatoires, Title = "Obligatoires" });

            return filtres;
        }

        [ErrorHandler]
        //public ActionResult ObtenirClauseDetails(string rubrique, string sousRubrique, string sequence, string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string etape, string tabGuid, string codeRisque, string codeFormule, string codeOption, string modeNavig, string filtreContext = contexteOrigine)
        public ActionResult ObtenirClauseDetails(string idClause, string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string etape, string tabGuid, string codeRisque, string codeFormule, string codeOption, string modeNavig, string filtreContext = contexteOrigine) {
            ModeleClause toReturn = new ModeleClause();
            // SAB : 30/09/2015


            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var client = channelClient.Channel;
                var wsClause = client.DetailsClauses(typeOffreContrat, codeOffreContrat, versionOffreContrat.ToString(), codeAvn.ToString(), etape, filtreContext, string.Empty, string.Empty, string.Empty, idClause, codeRisque, codeFormule, codeOption, string.Empty, modeNavig.ParseCode<ModeConsultation>());
                toReturn = ObjectMapperManager.DefaultInstance.GetMapper<ClauseDto, ModeleClause>().Map(wsClause);
                bool isReadOnly = GetIsReadOnly(GetSurroundedTabGuid(tabGuid), codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn.ToString());
                toReturn.IsReadOnlyMode = isReadOnly;
            }

            toReturn.NumAvenantPage = codeAvn;
            return PartialView("ClauseDetails", toReturn);
        }
        
        [ErrorHandler]
        public ActionResult ObtenirClauseVisualisation(string idClause, string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string etape, string tabGuid, string codeRisque, string codeFormule, string codeOption, string modeNavig, string filtreContext = contexteOrigine) {
            ModeleClause toReturn = new ModeleClause();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var client = channelClient.Channel;
                var result = client.ClausesGet2(typeOffreContrat, codeOffreContrat, versionOffreContrat.ToString(), codeAvn.ToString(), etape, filtreContext, string.Empty, string.Empty, string.Empty, idClause, codeRisque, codeFormule, codeOption, string.Empty, modeNavig.ParseCode<ModeConsultation>());
                if (result != null) {
                    toReturn = ObjectMapperManager.DefaultInstance.GetMapper<List<ClauseDto>, List<ModeleClause>>().Map(result.Clauses).FirstOrDefault();
                    bool isReadOnly = GetIsReadOnly(tabGuid, codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn.ToString());
                    toReturn.IsReadOnlyMode = isReadOnly;
                    if (result.Risques != null) {
                        var currRisque = result.Risques;
                        toReturn.Risque = codeRisque;
                        toReturn.DescRsq = currRisque.Designation;

                        var objets = new List<ModeleObjet>();
                        if (currRisque.Objets != null & currRisque.Objets.Count > 0) {
                            currRisque.Objets.ForEach(o => {
                                objets.Add(new ModeleObjet {
                                    Code = o.Code.ToString(),
                                    Designation = o.Designation
                                });
                            });
                        }
                        toReturn.ObjetsRisqueAll = new ModeleObjetsRisque {
                            Objets = objets
                        };
                        if (toReturn.ObjetsRisqueAll.Objets != null)
                            toReturn.NbrObjets = toReturn.ObjetsRisqueAll.Objets.Count;
                    }
                }


                if (!string.IsNullOrEmpty(toReturn.CodeObjet) && toReturn.CodeObjet != "0") {
                    toReturn.LibApplique = "S'applique à l'objet";
                    toReturn.DescRsqObj = string.Format("{0} - {1}", toReturn.CodeObjet, toReturn.DescObjet);
                    toReturn.IdObj = toReturn.CodeObjet;
                    toReturn.IsRsqSelected = false;
                }
                else {
                    toReturn.LibApplique = "S'applique au risque";
                    toReturn.DescRsqObj = string.Format("{0} - {1}", toReturn.Risque, toReturn.DescRsq);
                    toReturn.IdObj = string.Empty;
                    toReturn.IsRsqSelected = true;
                }
            }
            return PartialView("ClauseVisualisation", toReturn);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string tabGuid, string etape, string provenance, string contexte, string modeNavig, string addParamType, string addParamValue, bool isForceReadOnly) {
            if (!string.IsNullOrEmpty(cible)) {
                return RedirectToAction(job, cible, new { id = codeOffre + "_" + version + "_" + type + "_" + etape + "_" + provenance + "_" + contexte + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }
            var tProvenance = provenance.Split('/');
            var id = tProvenance[3];
            if (tProvenance[1] == "CreationFormuleGarantie") {
                string[] tId = id.Split('_');
                id = tId[0] + "_" + tId[1] + "_" + tId[2] + "_" + tId[3] + "_" + tId[4];
            }
            if (tProvenance[1] == "InformationsSpecifiquesGarantie") {
                id += "__";
            }
            return RedirectToAction(tProvenance[2], tProvenance[1], new { id = id + GetSurroundedTabGuid(tabGuid) + BuildAddParamString(addParamType, addParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty)) + GetFormatModeNavig(modeNavig) });
        }

        [ErrorHandler]
        public string UpdateCheckBox(string clauseId, bool isChecked) {
            //string etatTitre = isChecked ? "P" : "S";
            string situation = isChecked ? "V" : string.Empty;

            if (string.IsNullOrEmpty(clauseId))
                throw new AlbTechException(new Exception("Erreur lors de la mise à jour de la clause"));
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var client = channelClient.Channel;
                return client.UpdateEtatTitre(clauseId, situation);
            }
        }
        //SAB24042016: Pagination clause
        [ErrorHandler]
        public ActionResult LoadLstClause(string codeOffre, int version, string type, int codeAvn, string etape, string provenance, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, bool fullScreen = false) {
            var model = GetListClause(codeOffre, version, type, codeAvn, etape, provenance, tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig);
            model.FullScreen = fullScreen;
            return PartialView("ChoixClauseIntermediaire", model);
        }





        #region Liste des clauses transverse
        //Ouvre la liste des clause deupuis le menu transverse
        [ErrorHandler]
        [WhitespaceFilter]
        public ActionResult OpenListeClauses(string codeOffre, int version, string type, int codeAvn, string tabGuid, string modeNavig, string acteGestion) {
            var model = SetModeleChoixClauseTransverse(codeOffre, version, type, codeAvn, tabGuid, "Tous", string.Empty, string.Empty, string.Empty, AlbConstantesMetiers.ToutesSaufObligatoires, modeNavig, acteGestion);
            SetLayoutInfoTabModel(codeOffre, version, type, tabGuid, model);
            return PartialView("TransverseListeClauses", model);
        }

        [ErrorHandler]
        public ActionResult TransverseFiltrer(string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string etape, string tabGuid, string filtre, string modeNavig, string acteGestion) {
            var model = SetModeleChoixClauseTransverse(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeAvn, tabGuid, etape, string.Empty, string.Empty, string.Empty, filtre, modeNavig, acteGestion);
            SetLayoutInfoTabModel(codeOffreContrat, versionOffreContrat, typeOffreContrat, tabGuid, model);
            return PartialView("TransverseListeClauses", model);
        }
        [ErrorHandler]
        public ActionResult FilterParRisque(string codeOffre, int version, string typeOffre, int codeAvn, string codeRisque, string modeNavig, string acteGestion) {
            return PartialView("ListFormules", GetFormules(codeOffre, version, typeOffre, codeAvn, codeRisque, modeNavig, acteGestion));
        }
        [ErrorHandler]
        public ActionResult FilterParFormule(string codeOffre, int version, string typeOffre, int codeAvn, string codeFormule, string modeNavig, string acteGestion) {
            return PartialView("ListOptions", GetOptions(codeOffre, version, typeOffre, codeAvn, codeFormule, modeNavig, acteGestion));
        }
        [ErrorHandler]
        public ActionResult TransverseUpdateTextClauseLibre(string clauseId, string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string tabGuid, string etape, string titre, string texte, string modeNavig, string acteGestion) {
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn.ToString())) {
                string texteClauseLibre = Server.UrlDecode(texte);
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var client = channelClient.Channel;
                    client.UpdateTextClauseLibre(clauseId, titre, texteClauseLibre, string.Empty);
                }
            }
            var model = SetModeleChoixClauseTransverse(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeAvn, tabGuid, etape, string.Empty, string.Empty, string.Empty, AlbConstantesMetiers.ToutesSaufObligatoires, modeNavig, acteGestion);
            return PartialView("TransverseListeClauses", model);
        }

        //SAB24042016: Pagination clause
        [ErrorHandler]
        public ActionResult TransverseObtenirClauseVisualisation(string idClause, string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string etape, string tabGuid, string modeNavig, string filtreContext = contexteOrigine) {
            if (etape == "Tous") etape = string.Empty;
            ModeleClause toReturn = new ModeleClause();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var client = channelClient.Channel;
                ClauseDto wsClause = client.DetailsClauses(typeOffreContrat, codeOffreContrat, versionOffreContrat.ToString(), codeAvn.ToString(), etape, filtreContext, string.Empty, string.Empty, string.Empty, idClause, string.Empty, string.Empty, string.Empty, string.Empty, modeNavig.ParseCode<ModeConsultation>());
                toReturn = ObjectMapperManager.DefaultInstance.GetMapper<ClauseDto, ModeleClause>().Map(wsClause);
                bool isReadOnly = GetIsReadOnly(tabGuid, codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn.ToString());
                toReturn.IsReadOnlyMode = isReadOnly;
            }
            return PartialView("TransverseClauseVisualisation", toReturn);
        }

        //SAB24042016: Pagination clause

        [ErrorHandler]
        public ActionResult TransverseObtenirClauseDetails(string rubrique, string sousRubrique, string sequence, string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string tabGuid, string modeNavig, string filtreContext = contexteOrigine) {
            ModeleClause toReturn = new ModeleClause();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var client = channelClient.Channel;
                ClauseDto wsClause = client.DetailsClauses(typeOffreContrat, codeOffreContrat, versionOffreContrat.ToString(), codeAvn.ToString(), string.Empty, filtreContext, rubrique, sousRubrique, sequence, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, modeNavig.ParseCode<ModeConsultation>());
                toReturn = ObjectMapperManager.DefaultInstance.GetMapper<ClauseDto, ModeleClause>().Map(wsClause);
                bool isReadOnly = GetIsReadOnly("tabGuid" + tabGuid + "tabGuid", codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn.ToString());
                toReturn.IsReadOnlyMode = isReadOnly;
            }
            return PartialView("TransverseClauseDetails", toReturn);
        }

        #endregion

        [ErrorHandler]
        public string VerifAjout(string etape, string contexte, string typeAjt) {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                return serviceContext.VerifAjout(etape, contexte, typeAjt);
            }
        }

        [ErrorHandler]
        public ActionResult TrierClauses(string modeAffichage, string colTri, string codeOffre, string version, string type, string codeAvn, string etape, string provenance, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string imgTri)
        {
            var partialViewName = (modeAffichage == "Classique" ? "ListeChoixClauses" : "TransverseListeChoixClauses");
            etape = GetEtape(etape);
            return PartialView(partialViewName, GetClausesTriees(colTri, codeOffre, int.Parse(version), int.TryParse(codeAvn, out int a) ? a : default, type, etape, provenance, tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig, imgTri));
        }
        private List<ModeleClause> GetClausesTriees(string colTri, string codeOffre, int version, int codeAvn, string type, string etape, string provenance, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string imgTri = null)
        {
            var lstClause = GetClauses(type, codeOffre, version, codeAvn, etape, tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig);
            if (lstClause.Any())
            {
                //Mise à jour de l'origine
                lstClause.FindAll(elm => elm.Origine != "Utilisateur").ForEach(elm => elm.Origine = "Systeme");
                lstClause.FindAll(elm => elm.Origine == "Utilisateur" && elm.IsClauseLibre).ForEach(elm => elm.Origine = "Libre");
                lstClause.FindAll(elm => elm.Origine == "Utilisateur" && !elm.IsClauseLibre).ForEach(elm => elm.Origine = "Ajoutée");


                if (!string.IsNullOrEmpty(colTri))
                {
                    switch (colTri)
                    {
                        case "Risque":
                            if (imgTri == "tri_asc")
                            {
                                lstClause = lstClause.OrderByDescending(elm => elm.CodeRisque)
                                                     .ThenBy(elm => elm.CodeObjet)
                                                     .ThenBy(elm => elm.CodeFormule)
                                                     .ThenBy(elm => elm.Origine)
                                                     .ThenBy(elm => elm.Contexte)
                                                     .ThenBy(elm => elm.Edition)
                                                     .ThenBy(elm => elm.Rubrique)
                                                     .ThenBy(elm => elm.SousRubrique)
                                                     .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenBy(elm => elm.Titre).ToList();
                            }
                            else
                            {
                                lstClause = lstClause.OrderBy(elm => elm.CodeRisque)
                                                     .ThenBy(elm => elm.CodeObjet)
                                                     .ThenBy(elm => elm.CodeFormule)
                                                     .ThenBy(elm => elm.Origine)
                                                     .ThenBy(elm => elm.Contexte)
                                                     .ThenBy(elm => elm.Edition)
                                                     .ThenBy(elm => elm.Rubrique)
                                                     .ThenBy(elm => elm.SousRubrique)
                                                     .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenBy(elm => elm.Titre).ToList();
                            }
                            break;


                        case "NumInterne":
                            if (imgTri == "tri_asc")
                            {
                                lstClause = lstClause.OrderByDescending(elm => elm.NumeroAvenantModification)
                                                    .ThenBy(elm => elm.CodeRisque)
                                                    .ThenBy(elm => elm.CodeObjet)
                                                    .ThenBy(elm => elm.CodeFormule)
                                                    .ThenBy(elm => elm.Origine)
                                                    .ThenBy(elm => elm.Contexte)
                                                    .ThenBy(elm => elm.Edition)
                                                    .ThenBy(elm => elm.Rubrique)
                                                    .ThenBy(elm => elm.SousRubrique)
                                                    .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                    .ThenBy(elm => elm.Titre).ToList();
                            }
                            else
                            {
                                lstClause = lstClause.OrderBy(elm => elm.NumeroAvenantModification)
                                                   .ThenBy(elm => elm.CodeRisque)
                                                   .ThenBy(elm => elm.CodeObjet)
                                                    .ThenBy(elm => elm.CodeFormule)
                                                   .ThenBy(elm => elm.Origine)
                                                   .ThenBy(elm => elm.Contexte)
                                                   .ThenBy(elm => elm.Edition)
                                                   .ThenBy(elm => elm.Rubrique)
                                                   .ThenBy(elm => elm.SousRubrique)
                                                   .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                   .ThenBy(elm => elm.Titre).ToList();
                            }
                            break;
                        case "Titre":
                            if (imgTri == "tri_asc")
                            {
                                lstClause = lstClause.OrderByDescending(elm => elm.Origine)
                                                     .ThenByDescending(elm => elm.Rubrique)
                                                     .ThenByDescending(elm => elm.SousRubrique)
                                                     .ThenByDescending(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenByDescending(elm => elm.Titre).ToList();
                            }
                            else
                            {
                                lstClause = lstClause.OrderBy(elm => elm.Origine)
                                                     .ThenBy(elm => elm.Rubrique)
                                                     .ThenBy(elm => elm.SousRubrique)
                                                     .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenBy(elm => elm.Titre).ToList();
                            }
                            break;
                        case "Contexte":
                            if (imgTri == "tri_asc")
                            {
                                lstClause = lstClause.OrderByDescending(elm => elm.Contexte)
                                    .ThenByDescending(elm => elm.Edition)
                                    .ThenByDescending(elm => elm.Origine)
                                    .ThenByDescending(elm => elm.Rubrique)
                                    .ThenByDescending(elm => elm.SousRubrique)
                                    .ThenByDescending(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                    .ThenByDescending(elm => elm.Titre).ToList();
                            }
                            else
                            {
                                lstClause = lstClause.OrderBy(elm => elm.Contexte)
                                    .ThenBy(elm => elm.Edition)
                                    .ThenBy(elm => elm.Origine)
                                    .ThenBy(elm => elm.Rubrique)
                                    .ThenBy(elm => elm.SousRubrique)
                                    .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                    .ThenBy(elm => elm.Titre).ToList();
                            }
                            break;
                        case "Edition":
                            if (imgTri == "tri_asc")
                            {
                                lstClause = lstClause.OrderByDescending(elm => elm.Edition)
                                    .ThenByDescending(elm => elm.Origine)
                                    .ThenByDescending(elm => elm.Rubrique)
                                    .ThenByDescending(elm => elm.SousRubrique)
                                    .ThenByDescending(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                    .ThenByDescending(elm => elm.Titre).ToList();
                            }
                            else
                            {
                                lstClause = lstClause.OrderBy(elm => elm.Edition)
                                    .ThenBy(elm => elm.Origine)
                                    .ThenBy(elm => elm.Rubrique)
                                    .ThenBy(elm => elm.SousRubrique)
                                    .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                    .ThenBy(elm => elm.Titre).ToList();
                            }
                            break;
                    }
                }
            }
            return lstClause;
        }

        [ErrorHandler]
        public string SaveClauseLibre(string codeOffre, string version, string type, string codeAvt, string contexte, string etape, string codeRsq, string codeObj, string codeFor, string codeOpt,
            string emplacement, string sousemplacement, string ordre) {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                return serviceContext.SaveClauseLibre(codeOffre, version, type, codeAvt, contexte, etape, codeRsq, codeObj, codeFor, codeOpt, emplacement, sousemplacement, ordre);
            }
        }

        //SAB24042016: Pagination clause
        [ErrorHandler]
        public ActionResult UpdateDocumentLibre(string idClause, string idDoc, string codeOffre, int version, string type, int codeAvn, string etape, string provenance, string tabGuid,
            string codeRisque, string codeObjet, string codeFormule, string codeOption, string modeNavig, bool fullScreen) {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                serviceContext.UpdateDocumentLibre(idClause, idDoc, codeObjet, codeAvn.ToString());
            }
            ModeleChoixClause toReturn = new ModeleChoixClause();
            toReturn = GetListClause(codeOffre, version, type, codeAvn, etape, provenance, tabGuid, codeRisque, codeFormule, codeOption, AlbConstantesMetiers.ToutesSaufObligatoires, modeNavig);
            toReturn.FullScreen = fullScreen;
            return PartialView("ChoixClauseIntermediaire", toReturn);
        }


        [ErrorHandler]
        public string CreateDocumentLibre(string codeOffre, string version, string type, string etape, string idClause, string pathDoc, string nameDoc, string createDoc) {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                return serviceContext.CreateDocumentLibre(codeOffre, version, type, etape, idClause, pathDoc, nameDoc, createDoc);
            }
        }


        [ErrorHandler]
        public ActionResult LoadPiecesJointes(string codeOffre, string version, string type, string codeRisque, string etape, string contexte, string param) {
            ModeleChoixClausePieceJointe model = new ModeleChoixClausePieceJointe();

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.GetListPiecesJointes(codeOffre, version, type, codeRisque, "0", etape, contexte);
                model = (ModeleChoixClausePieceJointe)result;
            }

            model.Contexte = contexte;
            if (model.PiecesJointes == null) {
                model.PiecesJointes = new List<ModeleChoixClauseListePieceJointe>();
            }

            if (!string.IsNullOrEmpty(param)) {
                var tParam = param.Split('_');
                if (tParam != null && tParam.Length > 1) {
                    if (string.IsNullOrEmpty(model.Emplacement))
                        model.Emplacement = tParam[0];
                    if (string.IsNullOrEmpty(model.SousEmplacement))
                        model.SousEmplacement = tParam[1].ToUpper();
                    if (model.Ordre == 0)
                        model.Ordre = !string.IsNullOrEmpty(tParam[2]) ? Convert.ToInt32(tParam[2]) : 0;
                }
            }

            return PartialView("ClausePiecesJointes", model);
        }

        [ErrorHandler]
        public void SavePiecesJointes(string codeOffre, string version, string type, string contexte, string etape, string codeRsq, string codeObj, string codeFor, string codeOpt,
            string emplacement, string sousemplacement, string ordre, string piecesjointesId) {

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                //var result =
                serviceContext.SavePiecesJointes(codeOffre, version, type, contexte, etape, codeRsq, codeObj, codeFor, codeOpt, emplacement, sousemplacement, ordre, piecesjointesId);
            }
        }

        [ErrorHandler]
        public void SaveClauseEntete(string idClause, string emplacement, string sousemplacement, string ordre) {
            //Modifié le 2016-01-05 : demande de l'AMOA
            if (string.IsNullOrEmpty(idClause) || string.IsNullOrEmpty(emplacement) || string.IsNullOrEmpty(ordre)) {
                throw new AlbFoncException("L'emplacement et l'ordre sont obligatoires", trace: false, sendMail: false, onlyMessage: true);
            }
            else {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var serviceContext = channelClient.Channel;
                    serviceContext.SaveClauseEntete(idClause, emplacement, sousemplacement.ToUpper(), ordre);
                }
            }
        }

        [ErrorHandler]
        public ActionResult SaveClauseMagnetic(string codeAffaire, int version, string type, int codeAvn, string provenance, string tabGuid,
            string codeRisque, string codeFormule, string codeOption, string modeNavig, bool fullScreen,
            string emplacement, string sousemplacement, string ordre, string contexte,
            int idDoc, string acteGes, string etape, string nameClause, string fileName, int idClause) {
            if (string.IsNullOrEmpty(acteGes)) {
                switch (type) {
                    case "O":
                        acteGes = "OFFRE";
                        break;
                    case "P":
                        acteGes = "AFFNOUV";
                        break;
                }
            }

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                serviceContext.SaveClauseMagnetic(codeAffaire, version.ToString(), type, idDoc, acteGes, etape, nameClause, fileName, idClause, emplacement, sousemplacement, ordre, contexte);
            }

            ModeleChoixClause toReturn = new ModeleChoixClause();
            toReturn = GetListClause(codeAffaire, version, type, codeAvn, etape, provenance, tabGuid, codeRisque, codeFormule, codeOption, AlbConstantesMetiers.ToutesSaufObligatoires, modeNavig);
            toReturn.FullScreen = fullScreen;
            return PartialView("ChoixClauseIntermediaire", toReturn);
        }

        protected override void ExtendNavigationArbreRegule(MetaModelsBase contentData) {
            RegularisationNavigator.StandardInitContext(model, RegularisationStep.Cotisations);
            if (model?.Context != null) {
                RegularisationNavigator.Initialize(contentData.NavigationArbre, model.Context);
            }
        }

        protected override bool GetIsReadOnly(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "") {
            if (this.model.ChoixClauseIntermediaire?.Etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie)
                && this.model.ChoixClauseIntermediaire?.Etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque)) {
                return GetIsReadOnlyControllerBase(guid, currentFolder, numAvenant, isPopup, modeAvenant);
            }

            bool rdo = base.GetIsReadOnly(guid, currentFolder, numAvenant, isPopup);
            var folder = new Folder(currentFolder.Split('_'));
            int numAvn = int.TryParse(numAvenant, out int a) ? a : default;
            int rsq = int.TryParse(this.model.CodeRisque, out int r) ? r : default;
            int opt = int.TryParse(this.model.CodeOption, out int o) ? o : default;
            int frm = int.TryParse(this.model.CodeFormule, out int f) ? f : default;
            if (rdo || numAvn == 0 || folder.Type == AlbConstantesMetiers.TYPE_OFFRE || (rsq < 1 && opt < 1)) {
                return rdo;
            }
            var affaireId = folder.Adapt<AffaireId>();
            affaireId.NumeroAvenant = numAvn;
            if (this.model.AllParameters?.OriginPage?.ToUpper() == "DETAILSRISQUE") {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquePort>()) {
                    return client.Channel.IsAvnDisabled(affaireId, rsq);
                }
            }
            else if (this.model.AllParameters?.OriginPage?.ToUpper().IsIn("CONDITIONSGARANTIE", "INFORMATIONSSPECIFIQUESGARANTIE", "CREATIONFORMULEGARANTIE") == true) {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFormulePort>()) {
                    return client.Channel.IsAvnDisabled(affaireId, opt, frm);
                }
            }
            return rdo;
        }

        protected bool GetIsReadOnly(string guid, string currentFolder, int numAvenant = 0, bool isPopup = false, string modeAvenant = "") {
            return GetIsReadOnly(guid, currentFolder, numAvenant.ToString(), isPopup, modeAvenant);
        }

        #region Méthodes privées

        #region CR565 List Clauses (Formule et conditions tarifaires de garantie) mutualisation

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string ParseToJsonDataInfo(string id) {
            string codeOffre = (String.IsNullOrEmpty(model.CodePolicePage)) ? "" : model.CodePolicePage;
            string version = (String.IsNullOrEmpty(model.VersionPolicePage)) ? "0" : model.VersionPolicePage;
            string type = (String.IsNullOrEmpty(model.TypePolicePage)) ? "" : model.TypePolicePage;
            string codeFormule = string.Empty;
            string codeOption = string.Empty;

            StringBuilder sbCodeOffreVersTyp = new StringBuilder();
            sbCodeOffreVersTyp.Append(codeOffre);
            sbCodeOffreVersTyp.Append("_");
            sbCodeOffreVersTyp.Append(version);
            sbCodeOffreVersTyp.Append("_");
            sbCodeOffreVersTyp.Append(type);
            sbCodeOffreVersTyp.Append("_");

            string provenance = id.Replace(sbCodeOffreVersTyp.ToString(), "").Replace("¤", "/").Replace("£", "_");

            string[] tabProvenance = provenance.Split('/');

            if (tabProvenance.Length != 0) {
                string[] tabFormuleOption = tabProvenance.LastOrDefault().Replace(sbCodeOffreVersTyp.ToString(), "").Split('_');

                codeFormule = tabFormuleOption[0];
                if (tabFormuleOption.Length > 1)
                    codeOption = tabFormuleOption[1];
            }


            var dataInfo = new {
                txtSaveCancel = (String.IsNullOrEmpty(model.txtSaveCancel)) ? "0" : model.txtSaveCancel,
                txtParamRedirect = (String.IsNullOrEmpty(model.txtParamRedirect)) ? string.Empty : model.txtParamRedirect,
                CodeOffre = codeOffre,
                Version = version,
                Type = type,
                TabGuid = (String.IsNullOrEmpty(model.TabGuid)) ? string.Empty : model.TabGuid,
                AddParamType = (String.IsNullOrEmpty(model.AddParamType)) ? string.Empty : model.AddParamType,
                AddParamValue = (String.IsNullOrEmpty(model.AddParamValue)) ? string.Empty : model.AddParamValue,
                RisqueObj = (String.IsNullOrEmpty(model.RisqueObj)) ? string.Empty : model.RisqueObj,
                //provenance: '/' = '¤' et '_' = '£'
                Provenance = provenance,
                CodeFormule = codeFormule,
                CodeOption = codeOption,
                ContratIdentification = (String.IsNullOrEmpty(model.ContratIdentification)) ? string.Empty : model.ContratIdentification,
                ContratCible = (String.IsNullOrEmpty(model.ContratCible)) ? string.Empty : model.ContratCible,
                HasRisques = "True",
                ModeAvt = (String.IsNullOrEmpty(model.ModAvt)) ? string.Empty : model.ModAvt,
                TypeAvt = (String.IsNullOrEmpty(model.TypeAvt)) ? string.Empty : model.TypeAvt
            };




            var json = new JavaScriptSerializer().Serialize(dataInfo);

            return json;
        }

        private string ParseToJsonDataIntermediaire(string etape) {
            var etapeItem = new { Etape = etape };

            var json = new JavaScriptSerializer().Serialize(etapeItem);

            return json;
        }
        #endregion


        //SAB24042016: Pagination clause
        private ModeleChoixClauseTransverse SetModeleChoixClauseTransverse(string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, int codeAvn, string tabGuid, string etape, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string acteGestion) {
            var modelChoixClause = GetListClause(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeAvn, etape, string.Empty, tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig);
            var model = new ModeleChoixClauseTransverse {
                Etapes = modelChoixClause.Etapes,
                Etape = string.Empty,
                TableauClauses = modelChoixClause.TableauClauses,
                Contextes = modelChoixClause.Contextes,
                Provenance = string.Empty,
                Risques = GetRisques(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeAvn, modeNavig),
                DDLFormules = GetFormules(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeAvn, codeRisque, modeNavig, acteGestion),
                Filtres = modelChoixClause.Filtres,
                Filtre = modelChoixClause.Filtre
            };
            return model;
        }


        /// <summary>
        /// recupère tous les clauses, et charge la liste pour le filtre.
        /// </summary>
        /// <param name="typeOp"></param>
        /// <param name="codeOffre"></param>
        /// <param name="version"></param>
        /// <param name="filtreEtape"></param>
        /// <param name="filtreContext"></param>
        //SAB24042016: Pagination clause
        private List<ModeleClause> GetClauses(string typeOp, string codeOffre, int version, int codeAvn, string filtreEtape, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string filtreContext = contexteOrigine) {

            List<ModeleClause> toReturn = new List<ModeleClause>();
            List<ClauseDto> wsClause;
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var client = channelClient.Channel;
                wsClause = client.ClausesGet(typeOp, codeOffre, version.ToString(), codeAvn.ToString(), filtreEtape, filtreContext, string.Empty, string.Empty, string.Empty, string.Empty, codeRisque, codeFormule, codeOption, filtre, modeNavig.ParseCode<ModeConsultation>()).ToList();
            }
            toReturn = ObjectMapperManager.DefaultInstance.GetMapper<List<ClauseDto>, List<ModeleClause>>().Map(wsClause);
            bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + typeOp, codeAvn.ToString());
            toReturn.ForEach(m => m.IsReadOnlyMode = isReadOnly);
            return toReturn;
        }

        //SAB24042016: Pagination clause
        /// <summary>
        /// récupère les contextes des clauses de l'écran
        /// </summary>
        ///
        private List<AlbSelectListItem> GetContextes(List<ModeleClause> tableauClause) {
            // Liste des contexte (filtre)
            char[] split = { ' ', '-', ' ' };
            List<AlbSelectListItem> toReturn = new List<AlbSelectListItem>();
            toReturn = tableauClause.Select(x => x.Contexte + " - " + x.ContexteLabel).Distinct().Select(x => new AlbSelectListItem { Text = x, Value = x.Split(split)[0], Selected = false, Title = x }).ToList();
            toReturn.Insert(0, new AlbSelectListItem { Text = "Tous", Value = "Tous", Selected = false, Title = "Tous" });
            return toReturn;
        }

        private List<AlbSelectListItem> GetListeEtapes() {
            List<AlbSelectListItem> toReturn = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.GetListEtapes(OrigineAppel.Generale);
                if (result.Any()) {

                    toReturn = result.Where(m => m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.DeclencheurIncond) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Document) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Inventaire) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Saisie))
                        .Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();
                }
            }
            return toReturn;

        }
        private string GetEtape(string etape, string acteGestion = "", string acteGestionRegule = "") {
            if (etape == "InformationsSpecifiquesGarantie" || etape == "Garanties" || etape == "CreationFormuleGarantie" || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option)) {
                return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option);
            }
            if (etape == "ConditionsGarantie" || etape == "Conditions" || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie)) {
                return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie);
            }
            if (etape == "DetailsRisque" || etape == "Risque" || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque)) {
                return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque);
            }
            if (etape == "Objet" || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet)) {
                return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet);
            }
            if (etape == "FinOffre" || etape == "Quittance" || etape == "AnMontantReference" || etape == "Cotisations"
                || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin)
                || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.RemiseEnVigueur)) {
                if ((acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule != AlbConstantesMetiers.TYPE_AVENANT_MODIF) || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL) {
                    return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule);
                }
                if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_RESIL) {
                    return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Resiliation);
                }
                if (etape == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR || acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR) {
                    return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.RemiseEnVigueur);

                }
                return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin);
            }

            if (etape == "Tous" || string.IsNullOrEmpty(etape)) {
                etape = string.Empty;
                return etape;
            }

            if (etape == "Attestations" || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation)) {
                etape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation);
                return etape;
            }

            if (etape == "Regule" || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule)) {
                etape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule);
                return etape;
            }

            if (etape == "Resil" || etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Resiliation)) {
                etape = AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Resiliation);
                return etape;
            }

            if (etape != ContextStepName.EditionMontantsReference.AsCode()
                && etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoAssureur)
                && etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.CoCourtier)
                && etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Cotisation)
                && etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Engagement)
                && etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin)
                && etape != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option)) {
                return AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale);
            }
            else return etape;
        }

        private List<AlbSelectListItem> GetRisques(string codeOffre, int versionOffre, string typeOffre, int codeAvn, string modeNavig) {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var toReturn = new List<AlbSelectListItem>();
                //TODO : mettre l'avenant
                var wsRisques = serviceContext.ListRisquesObjet(codeOffre, versionOffre.ToString(), typeOffre, codeAvn.ToString(), modeNavig.ParseCode<ModeConsultation>());
                toReturn = wsRisques.Select(
                        r => new AlbSelectListItem {
                            Value = r.Code.ToString(),
                            Text = !string.IsNullOrEmpty(r.Code.ToString()) || !string.IsNullOrEmpty(r.Designation) ? string.Format("{0} - {1}", r.Code, r.Designation) : "",
                            Selected = false,
                            Title = !string.IsNullOrEmpty(r.Code.ToString()) || !string.IsNullOrEmpty(r.Designation) ? string.Format("{0} - {1}", r.Code, r.Designation) : "",
                        }).ToList();
                toReturn.Insert(0, new AlbSelectListItem { Text = "Tous", Value = "0", Selected = false, Title = "Tous" });
                return toReturn;
            }
        }
        private ModeleDDLFormules GetFormules(string codeOffre, int versionOffre, string typeOffre, int codeAvn, string codeRisque, string modeNavig, string acteGestion) {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var ddlFormules = new List<AlbSelectListItem>();
                if (!string.IsNullOrEmpty(codeRisque)) {
                    //TODO : mettre l'avenant
                    var wsFormule = serviceContext.InitMatriceFormule(codeOffre, versionOffre.ToString(), typeOffre, codeAvn.ToString(), modeNavig.ParseCode<ModeConsultation>(), GetUser(), acteGestion, model.IsReadOnly);
                    var risque = wsFormule.Risques.Find(r => r.Code == int.Parse(codeRisque));
                    if (risque != null && risque.Formules != null) {
                        ddlFormules = risque.Formules.Select(
                                f => new AlbSelectListItem {
                                    Value = f.Code.ToString(),
                                    Text = !string.IsNullOrEmpty(f.Code.ToString()) || !string.IsNullOrEmpty(f.Designation) ? string.Format("{0} - {1}", f.Code, f.Designation) : "",
                                    Selected = false,
                                    Title = !string.IsNullOrEmpty(f.Code.ToString()) || !string.IsNullOrEmpty(f.Designation) ? string.Format("{0} - {1}", f.Code, f.Designation) : "",
                                }).ToList();
                        ddlFormules.Insert(0, new AlbSelectListItem { Text = "Tous", Value = "0", Selected = false, Title = "Tous" });
                    }
                }
                var toReturn = new ModeleDDLFormules {
                    Formules = ddlFormules,
                    Formule = string.Empty,
                    DDLOptions = new ModeleDDLOptions {
                        Options = new List<AlbSelectListItem>(),
                        Option = string.Empty
                    }
                };
                return toReturn;
            }
        }
        private ModeleDDLOptions GetOptions(string codeOffre, int versionOffre, string typeOffre, int codeAvn, string codeFormule, string modeNavig, string acteGestion) {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var ddlOptions = new List<AlbSelectListItem>();
                if (!string.IsNullOrEmpty(codeFormule)) {
                    var wsFormule = serviceContext.InitMatriceFormule(codeOffre, versionOffre.ToString(), typeOffre, codeAvn.ToString(), modeNavig.ParseCode<ModeConsultation>(), GetUser(), acteGestion, model.IsReadOnly);
                    var formule = wsFormule.Formules.Find(f => f.Code == codeFormule);
                    if (formule != null && formule.Options != null) {
                        ddlOptions = formule.Options.Select(o => new AlbSelectListItem {
                            Value = o.Code.ToString(),
                            Text = !string.IsNullOrEmpty(o.Code.ToString()) || !string.IsNullOrEmpty(o.Designation) ? string.Format("{0} - {1}", o.Code, o.Designation) : "",
                            Selected = false,
                            Title = !string.IsNullOrEmpty(o.Code.ToString()) || !string.IsNullOrEmpty(o.Designation) ? string.Format("{0} - {1}", o.Code, o.Designation) : "",
                        }).ToList();
                        ddlOptions.Insert(0, new AlbSelectListItem { Text = "Tous", Value = "0", Selected = false, Title = "Tous" });
                    }
                }
                var toReturn = new ModeleDDLOptions {
                    Options = ddlOptions,
                    Option = string.Empty
                };
                return toReturn;
            }
        }

        private string GetArbreEtape() {
            string etape;
            switch (model.AllParameters.OriginPage) {
                case "InformationsSpecifiquesGarantie":
                case "ConditionsGarantie":
                case "CreationFormuleGarantie":
                    etape = "Formule";
                    break;
                case "DetailsRisque":
                    etape = "Risque";
                    break;
                case "Quittance":
                case "Cotisation":
                    etape = "Cotisation";
                    break;
                case "AnMontantReference":
                case "FinOffre":
                    etape = "Fin";
                    break;
                case "CreationAttestation":
                    etape = "Attestations";
                    break;
                case "Regule":
                    etape = "Regule";
                    break;
                default:
                    etape = "InfoGen";
                    break;
            }
            return etape;
        }

        /// <summary>
        /// Indique si le contexte est different du filtre.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool filtreContexte(ModeleClause s) {
            return (s.Contexte != leFiltre);
        }

        protected override void LoadInfoPage(string id) {
            var folder = model.AllParameters.Folder;
            LoadInfosBasesAffaire(folder);

            InfosBaseDto infosBaseAffaire = model.InfosBaseAffaire;

            if (folder?.Type == "O") {
                model.Offre = new Offre_MetaModel();
                model.Offre.LoadInfosOffre(infosBaseAffaire);
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                model.IsReadOnly = GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
            }
            else if (folder?.Type == "P") {
                model.Contrat = new ContratDto() {
                    CodeContrat = infosBaseAffaire.CodeOffre,
                    VersionContrat = Convert.ToInt64(infosBaseAffaire.Version),
                    Type = infosBaseAffaire.Type,
                    Branche = infosBaseAffaire.Branche.Code,
                    BrancheLib = infosBaseAffaire.Branche.Nom,
                    Cible = infosBaseAffaire.Branche.Cible.Code,
                    CibleLib = infosBaseAffaire.Branche.Cible.Nom,
                    CourtierGestionnaire = infosBaseAffaire.CabinetGestionnaire.Code,
                    Descriptif = infosBaseAffaire.Descriptif,
                    CodeInterlocuteur = infosBaseAffaire.CabinetGestionnaire.Code,
                    NomInterlocuteur = infosBaseAffaire.CabinetGestionnaire.Inspecteur,
                    CodePreneurAssurance = Convert.ToInt32(infosBaseAffaire.PreneurAssurance.Code),
                    NomPreneurAssurance = infosBaseAffaire.PreneurAssurance.NomAssure,
                    PeriodiciteCode = infosBaseAffaire.Periodicite,
                    Delegation = infosBaseAffaire?.CabinetGestionnaire?.Delegation?.Nom,
                    Inspecteur = infosBaseAffaire?.CabinetGestionnaire?.Inspecteur
                };


                var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);
                switch (model.TypeAvt) {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                        break;
                    case AlbConstantesMetiers.TYPE_ATTESTATION:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        if (regulMode == "PB") {
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;
                        }
                        else if (regulMode == "BNS") {
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBNS;
                        }
                        else if (regulMode == "BURNER") {
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER;
                        }
                        else {
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                        }
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when model.InfosBaseAffaire.IsRemiseEnVigeurSansModif:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when model.InfosBaseAffaire.IsRemiseEnVigeurAvecModif:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                        break;
                    default:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                        break;
                }
            }

            model.CodeOffre = folder?.CodeOffre;
            model.Version = folder?.Version.ToString();
            model.Type = folder?.Type;
            model.ModAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNMODE);
            model.ReguleId = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULEID);

            if (model.Offre != null || model.Contrat != null) {
                model.AfficherBandeau = DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }

            //Le cas d'une offre : 05/02/2013
            if (model.Offre != null) {
                model.ContratIdentification = model.Offre.Descriptif;
                if (model.Offre.Branche != null && model.Offre.Branche.Cible != null) {
                    model.ContratCible = model.Offre.Branche.Cible.Code;
                    model.ContratCibleLib = HttpUtility.UrlDecode(model.Offre.Branche.Cible.Nom);
                }
            }
            //Le cas d'un contrat : 05/02/2013
            else if (model.Contrat != null) {
                model.ContratIdentification = HttpUtility.UrlDecode(model.Contrat.Descriptif);
                model.ContratCible = model.Contrat.Cible;
                model.ContratCibleLib = model.Contrat.CibleLib;
            }
            model.ObjetDescriptif = new List<AlbSelectListItem>();
            model.Volet = new List<AlbSelectListItem>();
            model.Bloc = new List<AlbSelectListItem>();

            var version = String.Empty;
            if (Model.Offre != null) {
                Model.Navigation = new Navigation_MetaModel {
                    IdOffre = model.Offre.CodeOffre,
                    Version = model.Offre.Version
                };
                if (model.Offre.Version.HasValue) {
                    version = Convert.ToString(model.Offre.Version);
                }
                if (model.Offre.Risques != null && model.Offre.Risques.Count > 0 && model.Offre.Risques[0].Objets.Count > 0) {
                    model.RisqueObj = model.Offre.Risques[0].Objets[0].Descriptif;
                }
            }
            else if (Model.Contrat != null) {
                Model.Navigation = new Navigation_MetaModel {
                    IdOffre = model.Contrat.CodeContrat,
                    Version = int.Parse(model.Contrat.VersionContrat.ToString())
                };
                version = Convert.ToString(model.Contrat.VersionContrat);
                if (model.Contrat.Risques != null && model.Contrat.Risques.Count > 0 && model.Contrat.Risques[0].Objets.Count > 0) {
                    model.RisqueObj = model.Contrat.Risques[0].Objets[0].Descriptif;
                }
            }

            string etape = Model.AllParameters.OriginPage;
            switch (etape) {
                case "InformationsSpecifiquesGarantie":
                case "CreationFormuleGarantie":
                    string[] tabIdGar = model.Provenance.Split('_');
                    model.CodeFormule = tabIdGar[3];
                    model.CodeOption = tabIdGar[4];
                    using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                        var serviceContext = channelClient.Channel;
                        var result = serviceContext.GetLibFormule(Convert.ToInt32(model.CodeFormule), folder?.CodeOffre, folder?.Version.ToString(), folder?.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                        if (result != null && !string.IsNullOrEmpty(result)) {
                            model.LettreLibelleFormule = result.Split(new[] { "-" }, StringSplitOptions.None)[0];
                            model.LibelleFormule = result.Split(new[] { "-" }, StringSplitOptions.None)[1];
                        }
                    }

                    model.GarantieDescriptif = !string.IsNullOrEmpty(model.LibelleFormule) ? model.LettreLibelleFormule + "-" + model.LibelleFormule : model.LettreLibelleFormule;
                    model.Navigation.Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE;
                    using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                        var serviceContext = channelClient.Channel;
                        //TODO : mettre l'avenant
                        var result = serviceContext.GetCibleInfoFormule(model.CodeOffre, model.Version, model.Type, model.CodeFormule);
                        model.GarantieCible = result.Cible + " - " + result.DescCible;
                    }
                    break;
                case "ConditionsGarantie":
                    string[] tabIdCond = model.Provenance.Split('_');
                    model.CodeFormule = tabIdCond[3];
                    model.CodeOption = tabIdCond[4];

                    using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                        var serviceContext = channelClient.Channel;
                        var result = serviceContext.GetLibFormule(Convert.ToInt32(model.CodeFormule), folder?.CodeOffre, folder?.Version.ToString(), folder?.Type, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                        if (result != null && !string.IsNullOrEmpty(result)) {
                            model.LettreLibelleFormule = result.Split(new[] { "-" }, StringSplitOptions.None)[0];
                            model.LibelleFormule = result.Split(new[] { "-" }, StringSplitOptions.None)[1];
                        }
                    }

                    model.GarantieDescriptif = !string.IsNullOrEmpty(model.LibelleFormule) ? model.LettreLibelleFormule + "-" + model.LibelleFormule : model.LettreLibelleFormule;
                    model.Navigation.Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE;
                    using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                        var serviceContext = channelClient.Channel;
                        //TODO : mettre l'avenant
                        //var result = serviceContext.InitFormuleGarantie(model.CodeOffre, model.Version, model.Type, string.Empty, model.CodeFormule, model.CodeOption, string.Empty, model.ModeNavig.ParseCode<ModeConsultation>(), false, GetUser());
                        var result = serviceContext.GetCibleInfoFormule(model.CodeOffre, model.Version, model.Type, model.CodeFormule);
                        model.GarantieCible = result.Cible + " - " + result.DescCible;
                    }
                    break;
                case "DetailsRisque":
                    //etape = "Risque";
                    string[] tabIdRsq = model.Provenance.Split('_');
                    int idRsq = int.Parse(tabIdRsq[tabIdRsq.Length - 1]);
                    //Le cas d'une offre : 05/02/2012
                    if (model.Offre != null && model.Offre.Risques != null) {
                        RisqueDto rsq = model.Offre.Risques.Find(r => r.Code == idRsq);
                        model.RisqueCible = rsq.Cible.Code;
                        model.RisqueCibleLib = rsq.Cible.Nom;
                        model.RisqueDescriptif = rsq.Descriptif;
                    }
                    //Le cas d'un contrat : 05/02/2012
                    else if (model.Contrat != null && model.Contrat.Risques != null) {
                        RisqueDto rsq = model.Contrat.Risques.Find(r => r.Code == idRsq);
                        model.RisqueCible = rsq.Cible.Code;
                        model.RisqueCibleLib = rsq.Cible.Nom;
                        model.RisqueDescriptif = rsq.Descriptif;
                    }
                    model.CodeRisque = idRsq.ToString();
                    model.Navigation.Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE;
                    break;
               // case "FinOffre":
                   // model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOFIN;
                   // break;

                case "Cotisations":
                    model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOFIN;
                    break;
                case "AnMontantReference":
                case "Quittance":
                    if (model.ModeNavig == ModeConsultation.Historique.AsCode()) {
                        model.Navigation.Etape = Navigation_MetaModel.ECRAN_COTISATIONS;
                    }
                    else {
                        model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOFIN;
                    }
                    break;

                case "CreationAttestation":
                    etape = "Attestations";
                    break;

                default:
                    etape = "Informations générales";
                    model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOGENERALE;
                    break;
            }
            
            var acteGest = string.Empty;
            var acteGestRegule = string.Empty;
            var avnType = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || avnType == AlbConstantesMetiers.TYPE_AVENANT_REGUL) {
                acteGest = avnType;
                acteGestRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            }
            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_RESIL) {
                acteGest = avnType;
            }
            string etapeClause = GetEtape(etape, acteGest, acteGestRegule);

            model.ChoixClauseIntermediaire = GetListClause(model.CodeOffre, model.VersionNum,
                model.Type, model.NumAvenant, etape, model.Provenance,
                model.TabGuid, model.CodeRisque, model.CodeFormule, model.CodeOption,
                (etapeClause == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation) || etapeClause == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule))
                    ? AlbConstantesMetiers.Toutes : AlbConstantesMetiers.ToutesSaufObligatoires,
                model.ModeNavig);


            model.PageTitle = GetTitle();
        }

        private void LoadInfosBasesAffaire(Folder folder) {
            model.InfosBaseAffaire = GetInfoBaseAffaire(folder);
        }

        private InfosBaseDto GetInfoBaseAffaire(Folder folder) {
            InfosBaseDto infosBaseAffaire;
            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                var CommonOffreClient = chan.Channel;
                infosBaseAffaire = CommonOffreClient.LoadInfosBase(folder?.CodeOffre, folder?.Version.ToString(), folder?.Type, model.NumAvenantPage, model.ModeNavig);
            }

            return infosBaseAffaire;
        }

        private string GetTitle() {
            if (model.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Option)) {
                return string.Format("Choix des clauses - {0}", "Garanties");
            }
            if (model.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Garantie)) {
                return string.Format("Choix des clauses - {0}", "Conditions");
            }
            if (model.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Risque)) {
                return string.Format("Choix des clauses - {0}", "Risque");
            }
            if (model.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet)) {
                return string.Format("Choix des clauses - {0}", "Objet");
            }
            if (model.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.InfoGenerale)) {
                return string.Format("Choix des clauses - {0}", "BASE");
            }
            if (model.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Fin)) {
                return string.Format("Choix des clauses - {0}", "Fin");
            }
            if (model.ChoixClauseIntermediaire.Etape == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation)) {
                return string.Format("Choix des Clauses - {0}", "Attestation");
            }
            return string.Format("Choix des clauses - {0}", model.ChoixClauseIntermediaire.Etape);

        }

        private void SetArbreNavigation() {
            var codeRisque = 0;
            var codeFormule = 0;
            var etape = GetArbreEtape();
            var tProvenance = model.Provenance.Split('_');
            //model.ActeGestion = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            if (tProvenance.Length > 3) {
                if (etape == "Risque")
                    codeRisque = Convert.ToInt32(tProvenance[3]);
                if (etape == "Formule")
                    codeFormule = Convert.ToInt32(tProvenance[3]);
            }
            if (model.Offre != null) {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre(etape, codeRisque, codeFormule);
            }
            else if (model.Contrat != null) {
                switch (etape) {
                    case "Attestations":
                        model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie", returnEmptyTree: true);
                        break;
                    case "Cotisation":
                        if (((model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL || model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF)
                            && model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_MODIF) ||
                            (model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_MODIF && model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_MODIF)) {
                            model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Cotisation");
                        }
                        else {
                            if ((!String.IsNullOrEmpty(model.ActeGestion) && !String.IsNullOrEmpty(model.ActeGestionRegule))
                                && ((model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGUL && model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL) ||
                                 (model.ActeGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && model.ActeGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL))) {
                                model.NavigationArbre = GetNavigationArbreRegule(model, "Cotisation");
                                model.NavigationArbre.IsRegule = true;
                                break;
                            }
                            else {
                                model.NavigationArbre = GetNavigationArbreAffaireNouvelle(etape, codeRisque, codeFormule);
                            }
                        }
                        break;
                    default:
                        model.NavigationArbre = GetNavigationArbreAffaireNouvelle(etape, codeRisque, codeFormule);
                        break;
                }
            }

            if (model.NavigationArbre != null) {
                model.NavigationArbre.ListeClauses = true;
            }
        }

        private void SetBandeauNavigation(string type) {
            if (model.AfficherBandeau) {
                model.Bandeau = GetInfoBandeau(type);
                //Gestion des Etapes
                if (model.Offre != null) {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                }
                else if (model.Contrat != null) {
                    var affaire = GetInfoBaseAffaire(model.CodePolicePage, model.VersionPolicePage, model.TypePolicePage, model.NumAvenant.ToString(), model.ModeNavig);
                    var regulMode = GetAddParamValue(model.AddParamValue, AlbParameterName.REGULMOD);
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt) {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_ATTESTATION:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_ATTES;
                            break;

                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            if (regulMode == "PB") {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULPB;
                            }
                            else if (regulMode == "BNS") {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBNS;
                            }
                            else if (regulMode == "BURNER") {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULBURNER;
                            }
                            else {
                                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            }
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurSansModif:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR_NO_MODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR when affaire.IsRemiseEnVigeurAvecModif:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                }
            }
        }
        private RedirectToRouteResult SuivantGaranties(ChoixClauses_Index_MetaModel listClauses, bool isForceReadOnly) {
            #region CR565 List Clauses (Formule et conditions tarifaires de garantie) mutualisation

            if (!listClauses.ModeNavig.StartsWith(ModeNAVIG)) {
                listClauses.ModeNavig = string.Format("{0}{1}", ModeNAVIG, listClauses.ModeNavig);
            }

            if (!listClauses.ModeNavig.EndsWith(ModeNAVIG)) {
                listClauses.ModeNavig = string.Format("{0}{1}", listClauses.ModeNavig, ModeNAVIG);
            }

            #endregion

            if (!string.IsNullOrEmpty(listClauses.txtParamRedirect)) {
                var tabParamRedirect = listClauses.txtParamRedirect.Split('/');
                var idInfo = tabParamRedirect[2].Contains("modeNavig") ? tabParamRedirect[2] : tabParamRedirect[2] + GetFormatModeNavig(listClauses.ModeNavig);
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = idInfo });
            }

            return RedirectToAction("Index", "ConditionsGarantie", new {
                id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + "_" + listClauses.CodeFormule +
                    "_" + listClauses.CodeOption + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue + (isForceReadOnly ? "||FORCEREADONLY|1" : string.Empty)) + GetFormatModeNavig(listClauses.ModeNavig),
                returnHome = listClauses.txtSaveCancel
            });
        }
        private RedirectToRouteResult SuivantConditions(ChoixClauses_Index_MetaModel listClauses, bool isModeConsultationEcran, bool isForceReadOnly) {
            if (!string.IsNullOrEmpty(listClauses.txtParamRedirect)) {
                var tabParamRedirect = listClauses.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            var numAvn = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNID);
            var isOffreReadonly = GetIsReadOnly(listClauses.TabGuid, listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type, numAvn);
            return RedirectToAction("Index", "MatriceFormule", new {
                id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue + ((isModeConsultationEcran || isOffreReadonly) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1")) + GetFormatModeNavig(listClauses.ModeNavig),
                returnHome = listClauses.txtSaveCancel,
                guidTab = listClauses.TabGuid
            });
        }

        private RedirectToRouteResult SuivantFinOffre(ChoixClauses_Index_MetaModel listClauses) {
            if (!string.IsNullOrEmpty(listClauses.txtParamRedirect)) {
                var tabParamRedirect = listClauses.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[0] == "ControleFin" ? tabParamRedirect[2].Replace(listClauses.Type + "tabGuid", listClauses.Type + "_GtabGuid") : tabParamRedirect[2] });
            }
            var numAvn = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNID);
            var acteGestionRegule = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);

            if (acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL) {
                return RedirectToAction("Index", "AvenantInfoGenerales", new { id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue) + GetFormatModeNavig(listClauses.ModeNavig), returnHome = listClauses.txtSaveCancel, guidTab = listClauses.TabGuid });
            }

            var folder = string.Format("{0}_{1}_{2}", listClauses.CodeOffre, listClauses.Version, listClauses.Type);
            var isModifHorsAvn = GetIsModifHorsAvn(listClauses.TabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(numAvn) ? "0" : numAvn));

            if (!GetIsReadOnly(listClauses.TabGuid, listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type, numAvn) || isModifHorsAvn)
                return RedirectToAction("Index", "ControleFin", new { id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + "_G" + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue) + GetFormatModeNavig(listClauses.ModeNavig), returnHome = listClauses.txtSaveCancel, guidTab = listClauses.TabGuid });
            else
                return RedirectToAction("Index", "DocumentGestion", new { id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue) + GetFormatModeNavig(listClauses.ModeNavig), returnHome = listClauses.txtSaveCancel, guidTab = listClauses.TabGuid });

        }
        private RedirectToRouteResult SuivantAttestation(ChoixClauses_Index_MetaModel listClauses) {
            return RedirectToAction("Index", "DocumentGestion", new { id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue) + GetFormatModeNavig(listClauses.ModeNavig) });
        }
        private RedirectToRouteResult SuivantFinregul(ChoixClauses_Index_MetaModel listClauses) {
            if (!string.IsNullOrEmpty(listClauses.txtParamRedirect)) {
                var tabParamRedirect = listClauses.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[0] == "ControleFin" ? tabParamRedirect[2].Replace(listClauses.Type + "tabGuid", listClauses.Type + "_GtabGuid") : tabParamRedirect[2] });
            }
            var acteGestion = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNTYPE);
            var acteGestionRegule = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);

            if (acteGestion == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF && acteGestionRegule == AlbConstantesMetiers.TYPE_AVENANT_REGUL) {
                return RedirectToAction("Index", "DocumentGestion", new { id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue) + GetFormatModeNavig(listClauses.ModeNavig), returnHome = listClauses.txtSaveCancel, guidTab = listClauses.TabGuid });
            }
            return RedirectToAction("Index", "ControleFin", new { id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + "_G" + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue) + GetFormatModeNavig(listClauses.ModeNavig), returnHome = listClauses.txtSaveCancel, guidTab = listClauses.TabGuid });
        }
        private RedirectToRouteResult OpenAvnModif(ChoixClauses_Index_MetaModel listClauses) {

            string addParam = string.Empty;
            string workParam = string.Empty;
            var codeAvn = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNID);
            if (!string.IsNullOrEmpty(listClauses.ModeAvt)) {
                workParam += "||" + AlbParameterName.AVNMODE + "|" + listClauses.ModeAvt;
            }

            if (!string.IsNullOrEmpty(listClauses.TypeAvt)) {
                workParam += "||" + AlbParameterName.AVNTYPE + "|" + listClauses.TypeAvt;
            }

            if (listClauses.ModeAvt == "UPDATE") {
                workParam += "||" + AlbParameterName.AVNID + "|" + (string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn);
                workParam += "||" + AlbParameterName.AVNIDEXTERNE + "|" + (string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn);
            }
            workParam += "||" + AlbParameterName.REGULEID + "|" + (string.IsNullOrEmpty(listClauses.ReguleId) ? "0" : listClauses.ReguleId);

            if (!string.IsNullOrEmpty(workParam))
                addParam = "addParam" + AlbOpConstants.GLOBAL_TYPE_ADD_PARAM_AVN + "|||" + workParam.Substring(2) + "addParam";

            string paramContrat = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type;

            return RedirectToAction("Index", "CreationAvenant", new { id = string.Concat(paramContrat, listClauses.TabGuid, addParam, GetFormatModeNavig(listClauses.ModeNavig)), returnHome = listClauses.txtSaveCancel, guidTab = listClauses.TabGuid });
        }

        private RedirectToRouteResult SuivantDefault(ChoixClauses_Index_MetaModel listClauses, bool isModeConsultationEcran, bool isForceReadOnly) {
            if (!string.IsNullOrEmpty(listClauses.txtParamRedirect)) {
                var tabParamRedirect = listClauses.txtParamRedirect.Split('/');
                var idInfo = tabParamRedirect[2].Contains("modeNavig") ? tabParamRedirect[2] : tabParamRedirect[2] + GetFormatModeNavig(listClauses.ModeNavig);
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = idInfo });
            }

            var numAvn = GetAddParamValue(listClauses.AddParamValue, AlbParameterName.AVNID);
            var isOffreReadonly = GetIsReadOnly(listClauses.TabGuid, listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type, numAvn);
            string[] tbProvenance = null;
            if (!string.IsNullOrEmpty(listClauses.Provenance))
                tbProvenance = listClauses.Provenance.Split('/');

            if (listClauses.Type == "O" || (listClauses.Type == "P" && tbProvenance != null && tbProvenance[1] == "DetailsRisque")) {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var serviceContext = channelClient.Channel;
                    var codeRsq = serviceContext.GetFirstCodeRsq(listClauses.CodeOffre, listClauses.Version, listClauses.Type);
                    var codeObj = serviceContext.GetFirstCodeObjRsq(listClauses.CodeOffre, listClauses.Version, listClauses.Type, codeRsq);
                    return RedirectToAction("Index", listClauses.HasRisques ? "MatriceRisque"
                      : "DetailsObjetRisque", new {
                          id = listClauses.HasRisques ?
                              listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue + ((isModeConsultationEcran || isOffreReadonly) && !isForceReadOnly ? string.Empty : "||IGNOREREADONLY|1")) + GetFormatModeNavig(listClauses.ModeNavig)
                              : listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type + "_" + codeRsq + "_" + codeObj + GetSurroundedTabGuid(listClauses.TabGuid) + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue) + GetFormatModeNavig(listClauses.ModeNavig),
                          returnHome = listClauses.txtSaveCancel,
                          guidTab = listClauses.TabGuid
                      });
                }
            }

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                return RedirectToAction(
                    "Index",
                    channelClient.Channel.ExistCoAs(listClauses.CodeOffre, listClauses.Version, listClauses.Type, listClauses.ModeNavig.ParseCode<ModeConsultation>())
                        ? "AnCoAssureurs"
                        : "AnCourtier",
                    new {
                        id = listClauses.CodeOffre + "_" + listClauses.Version + "_" + listClauses.Type
                            + GetSurroundedTabGuid(listClauses.TabGuid)
                            + BuildAddParamString(listClauses.AddParamType, listClauses.AddParamValue)
                            + GetFormatModeNavig(listClauses.ModeNavig),
                        returnHome = listClauses.txtSaveCancel,
                        guidTab = listClauses.TabGuid
                    });
            }
        }
        //SAB24042016: Pagination clause
        private ModeleChoixClause GetListClause(string codeOffre, int version, string type, int codeAvn, string etape, string provenance, string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string acteGestion = "") {
            var acteGest = string.Empty;
            var acteGestRegule = string.Empty;
            var avnType = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || avnType == AlbConstantesMetiers.TYPE_AVENANT_REGUL) {
                acteGest = avnType;
                acteGestRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            }
            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_RESIL)
            {
                acteGest = avnType;
            }

            if (avnType == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR)
            {
                acteGest = avnType;
                acteGestRegule = GetAddParamValue(model.AddParamValue, AlbParameterName.ACTEGESTIONREGULE);
            }

            string etapeClause = GetEtape(etape, acteGest, acteGestRegule);

            ModeleChoixClause model2 = GetInfoChoixClauses("Contexte", codeOffre, version, type, codeAvn, etape, provenance, tabGuid, codeRisque, codeFormule, codeOption, filtre, modeNavig, acteGest, acteGestRegule);
            model2.Contextes = GetContextes(model2.TableauClauses);
            model2.Provenance = provenance;
            model2.Filtres = GetFiltres(filtre);
            model2.Filtre = filtre;



            return model2;
        }

        //SAB24042016: Pagination clause
        private ModeleChoixClause GetInfoChoixClauses(string colTri, string codeOffre, int version, string type, int codeAvn, string etape, string provenance,
                        string tabGuid, string codeRisque, string codeFormule, string codeOption, string filtre, string modeNavig, string acteGest = "", string acteGestRegule = "", string filtreContext = contexteOrigine, string imgTri = null) {
            ModeleChoixClause toReturn = new ModeleChoixClause();

            (string etapeSearch, OrigineAppel origine) =
                ComputeEtapeAndOrigin(codeOffre, version, type, codeAvn, etape, acteGest, acteGestRegule);

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.GetInfoChoixClause(origine, type, codeOffre, version.ToString(), codeAvn.ToString(), etapeSearch, filtreContext,
                                                                string.Empty, string.Empty, string.Empty, string.Empty, codeRisque, codeFormule, codeOption, filtre,
                                                                modeNavig.ParseCode<ModeConsultation>(), etapeSearch, "KHEOP", "CTX");

                if (result != null) {
                    toReturn = LoadInfoChoixClauses(result, tabGuid, codeOffre, version, type, codeAvn, colTri, imgTri);
                }

            }
            toReturn.Etape = etapeSearch;

            return toReturn;
        }

        private (string etapeSearch, OrigineAppel origine) ComputeEtapeAndOrigin(string codeOffre, int version, string type, int codeAvn, string etape, string acteGest = null, string acteGestRegule = "") {
            string etapeSearch;
            OrigineAppel origine;
            LoadInfoBasesAffaire(codeOffre, version, type, codeAvn);
            if (acteGest == null) {
                acteGest = model.InfosBaseAffaire.TypeTraitement;
            }
            var acte = !string.IsNullOrEmpty(acteGestRegule) ? acteGestRegule : acteGest;

            if (codeAvn > 1 && model.InfosBaseAffaire.IsRemiseEnVigeurAvecModif && acteGest == AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR) {
                acte = AlbConstantesMetiers.TYPE_AVENANT_MODIF;
            }

            etapeSearch = GetEtape(etape, acte);

            origine = etape == AlbEnumInfoValue.GetEnumInfo(OrigineAppel.Attestation) ? OrigineAppel.Attestation :
                    etape == AlbEnumInfoValue.GetEnumInfo(OrigineAppel.Regule) ? OrigineAppel.Regule :
                    OrigineAppel.Generale;

            // 20160322 : Ajout du test de l'étape pour gérer les attestations.
            if (etapeSearch == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Attestation)) {
                origine = OrigineAppel.Attestation;
            }

            if (etapeSearch == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Regule)) {
                origine = OrigineAppel.Regule;
            }

            if (etapeSearch == AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Resiliation)) {
                origine = OrigineAppel.Resil;
            }
            return (etapeSearch, origine);
        }

        private void LoadInfoBasesAffaire(string codeOffre, int version, string type, int codeAvn) {
            if (model.InfosBaseAffaire == null) {
                char typefolder = type[0];

                LoadInfosBasesAffaire(new Folder(codeAffaire: codeOffre, version: version, type: typefolder, numeroAvenant: codeAvn));
            }
        }

        //SAB24042016: Pagination clause
        private ModeleChoixClause LoadInfoChoixClauses(ChoixClausesInfoDto result, string tabGuid, string codeOffre, int version, string type, int numAvn, string colTri, string imgTri = null) {
            ModeleChoixClause toReturn = new ModeleChoixClause();

            if (result.Etapes.Any())
                toReturn.Etapes = result.Etapes.Where(m => m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.DeclencheurIncond) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Document) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Inventaire) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Objet) && m.Code != AlbEnumInfoValue.GetEnumInfo(AlbConstantesMetiers.Etapes.Saisie))
                    .Select(m => new AlbSelectListItem { Value = m.Code, Text = m.Code + " - " + m.Libelle, Selected = false, Title = m.Libelle }).ToList();

            var lstClause = new List<ModeleClause>();
            result.Clauses.ForEach(elm => lstClause.Add((ModeleClause)elm));
            bool isReadOnly = GetIsReadOnly(tabGuid, codeOffre + "_" + version + "_" + type, numAvn);
            lstClause.ForEach(m => m.IsReadOnlyMode = isReadOnly);
            lstClause.ForEach(m => m.IsModifHorsAvenant = model.IsModifHorsAvenant);

            if (lstClause.Any()) {
                //Mise à jour de l'origine
                lstClause.FindAll(elm => elm.Origine != "Utilisateur" && elm.Origine != "PJ").ForEach(elm => elm.Origine = "Systeme");
                lstClause.FindAll(elm => elm.Origine == "Utilisateur" && elm.IsClauseLibre).ForEach(elm => elm.Origine = "Libre");
                lstClause.FindAll(elm => elm.Origine == "Utilisateur" && !elm.IsClauseLibre).ForEach(elm => elm.Origine = "Ajoutée");
                lstClause.FindAll(elm => elm.Origine == "PJ").ForEach(elm => elm.Origine = "PJ");


                if (!string.IsNullOrEmpty(colTri)) {
                    switch (colTri) {
                        case "Risque":
                            lstClause = lstClause.OrderBy(elm => elm.CodeRisque)
                                                 .ThenBy(elm => elm.CodeObjet)
                                                 .ThenBy(elm => elm.CodeFormule)
                                                 .ThenBy(elm => elm.Origine)
                                                 .ThenBy(elm => elm.Rubrique)
                                                 .ThenBy(elm => elm.SousRubrique)
                                                 .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                 .ThenBy(elm => elm.Titre).ToList();
                            break;
                        case "Titre":
                            if (imgTri == "tri_asc") {
                                lstClause = lstClause.OrderByDescending(elm => elm.Origine)
                                                     .ThenByDescending(elm => elm.Rubrique)
                                                     .ThenByDescending(elm => elm.SousRubrique)
                                                     .ThenByDescending(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenByDescending(elm => elm.Titre).ToList();
                            }
                            else {
                                lstClause = lstClause.OrderBy(elm => elm.Origine)
                                                     .ThenBy(elm => elm.Rubrique)
                                                     .ThenBy(elm => elm.SousRubrique)
                                                     .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenBy(elm => elm.Titre).ToList();
                            }
                            break;
                        case "Contexte":
                            lstClause = lstClause.OrderBy(elm => elm.Contexte)
                                                 .ThenBy(elm => elm.Edition)
                                                 .ThenBy(elm => elm.Origine)
                                                 .ThenBy(elm => elm.Rubrique)
                                                 .ThenBy(elm => elm.SousRubrique)
                                                 .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                 .ThenBy(elm => elm.Titre).ToList();
                            break;
                        case "Edition":
                            if (imgTri == "tri_asc") {
                                lstClause = lstClause.OrderBy(elm => elm.Edition)
                                                     .ThenBy(elm => elm.Origine)
                                                     .ThenBy(elm => elm.Rubrique)
                                                     .ThenBy(elm => elm.SousRubrique)
                                                     .ThenBy(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenBy(elm => elm.Titre).ToList();
                            }
                            else {
                                lstClause = lstClause.OrderByDescending(elm => elm.Edition)
                                                     .ThenByDescending(elm => elm.Origine)
                                                     .ThenByDescending(elm => elm.Rubrique)
                                                     .ThenByDescending(elm => elm.SousRubrique)
                                                     .ThenByDescending(elm => (!string.IsNullOrEmpty(elm.Sequence) ? Convert.ToInt32(elm.Sequence) : 0))
                                                     .ThenByDescending(elm => elm.Titre).ToList();
                            }
                            break;
                    }
                }
            }
            toReturn.TableauClauses = lstClause;


            toReturn.ContextesCibles = new List<AlbSelectListItem>();
            toReturn.ContextesCiblesCode = new List<AlbSelectListItem>();
            if (result.ContextesCibles.Any() && (result.ContextesCibles.Count > 0)) {
                toReturn.ContextesCibles = result.ContextesCibles
                    .Select(p => new AlbSelectListItem { Text = string.Format("{0} - {1}", p.Code, p.Libelle), Value = p.Code, Selected = false, Title = !string.IsNullOrEmpty(p.Code) ? string.Format("{0} - {1}", p.Code, p.Libelle) : "" }).ToList();
                toReturn.ContextesCiblesCode = result.ContextesCibles
                  .Select(p => new AlbSelectListItem { Text = p.Code, Title = p.Code, Value = p.LongId.ToString(), Selected = false }).ToList();
            }

            toReturn.TableauClauses.ForEach(elm => elm.NumAvenantPage = numAvn);

            return toReturn;
        }

        #endregion

        #region Clause Libre
        [ErrorHandler]
        public ActionResult AfficherEcranClauseLibre(string codeOffreContrat, int versionOffreContrat, string typeOffreContrat, string codeRisque, string provenance, string etape, string contexte) {
            var toReturn = new ModeleClauseLibre { Contexte = contexte, IsRsqSelected = false };
            if (!string.IsNullOrEmpty(codeRisque)) {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var serviceContext = channelClient.Channel;
                    var result = serviceContext.GetRisque(codeOffreContrat, versionOffreContrat.ToString(), typeOffreContrat, codeRisque);

                    if (result != null) {
                        toReturn.Risque = codeRisque;
                        toReturn.DescRsq = result.Designation;

                        var objets = new List<ModeleObjet>();
                        if (result.Objets != null && result.Objets != null & result.Objets.Count > 0) {
                            if (result.Objets.Count > 1)
                                toReturn.IsRsqSelected = true;
                            result.Objets.ForEach(o => objets.Add(new ModeleObjet {
                                Code = o.Code.ToString(CultureInfo.InvariantCulture),
                                Designation = o.Designation
                            }));
                        }
                        toReturn.ObjetsRisqueAll = new ModeleObjetsRisque {
                            Objets = objets
                        };
                        if (toReturn.ObjetsRisqueAll.Objets != null)
                            toReturn.NbrObjets = toReturn.ObjetsRisqueAll.Objets.Count;
                    }
                }
            }

            return PartialView("ClauseLibre", toReturn);

        }

        //SAB24042016: Pagination clause
        [ErrorHandler]
        public ActionResult EnregistrerClauseLibre(
            string codeOffreContrat,
            int versionOffreContrat,
            string typeOffreContrat,
            int codeAvn,
            string tabGuid,
            string provenance,
            string contexte,
            string etape,
            string libelle,
            string texte,
            string codeRisque,
            string codeFormule,
            string codeOption,
            string codeObj,
            string modeNavig,
            bool fullScreen,
            string filtre,
            string acteGestion,
            string acteGestionRegule,
            string colTri,
            string imgTri) {
            ModeleChoixClause toReturn = new ModeleChoixClause();
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn.ToString())) {
                var texteClauseLibre = Server.UrlDecode(texte);

                if (texteClauseLibre.Trim().Length > 5000)
                    throw new AlbFoncException(string.Format("Le texte ne doit pas dépasser 5000 caractères ({0})", texteClauseLibre.Trim().Length), trace: false, sendMail: false, onlyMessage: true);
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var client = channelClient.Channel;
                    var result = client.EnregistreClauseLibre(codeOffreContrat, versionOffreContrat.ToString(), typeOffreContrat, contexte, etape, codeRisque, codeFormule, codeOption, codeObj, libelle, texteClauseLibre);
                    if (!string.IsNullOrEmpty(result))
                        throw new AlbFoncException(result);
                }
            }
            return this.Filtrer(
                codeOffreContrat,
                versionOffreContrat,
                typeOffreContrat,
                codeAvn,
                etape,
                provenance,
                fullScreen,
                tabGuid,
                codeRisque,
                codeFormule,
                codeOption,
                filtre,
                modeNavig,
                acteGestion,
                acteGestionRegule,
                false, false, 
                colTri,
                imgTri);
        }


        ////Obsolète
        //[AjaxException]
        public ActionResult UpdateTextClauseLibre(
            string clauseId,
            string codeOffreContrat,
            int versionOffreContrat,
            string typeOffreContrat,
            int codeAvn,
            string tabGuid,
            string provenance,
            string etape,
            string titre,
            string texte,
            string codeRisque,
            string codeFormule,
            string codeOption,
            string codeObj,
            string modeNavig,
            bool fullScreen,
            string filtre,
            string acteGestion,
            string acteGestionRegule,
            string colTri,
            string imgTri) {
            ModeleChoixClause toReturn = new ModeleChoixClause();
            //Sauvegarde uniquement si l'écran n'est pas en readonly
            if (!GetIsReadOnly(tabGuid, codeOffreContrat + "_" + versionOffreContrat + "_" + typeOffreContrat, codeAvn.ToString())) {
                string texteClauseLibre = Server.UrlDecode(texte);
                if (texteClauseLibre.Trim().Length > 5000)
                    throw new AlbFoncException(string.Format("Le texte ne doit pas dépasser 5000 caractères ({0})", texteClauseLibre.Trim().Length), trace: false, sendMail: false, onlyMessage: true);
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    var client = channelClient.Channel;
                    client.UpdateTextClauseLibre(clauseId, titre, texteClauseLibre.Trim(), codeObj);
                }
            }
            return this.Filtrer(
                codeOffreContrat,
                versionOffreContrat,
                typeOffreContrat,
                codeAvn,
                etape,
                provenance,
                fullScreen,
                tabGuid,
                codeRisque,
                codeFormule,
                codeOption,
                filtre,
                modeNavig,
                acteGestion,
                acteGestionRegule,
                false, false,
                colTri,
                imgTri);

        }

        [ErrorHandler]
        public ActionResult GetInfoClauseLibreViewer(string codeOffre, string version, string type, string codeRsq, string clauseId, string clauseType, string createClause, string etape, string contexte, bool isReadonly) {
            ModeleViewerClauseLibre model = new ModeleViewerClauseLibre {
                Emplacements = new List<AlbSelectListItem>()
            };

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.GetInfoClauseLibreViewer(codeOffre, version, type, codeRsq, clauseId, etape, contexte);

                if (result != null) {
                    model = (ModeleViewerClauseLibre)result;
                    model.Emplacements = result.Emplacements.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();

                    if (result.Risque != null) {
                        var curRsq = result.Risque;
                        model.Risque = curRsq.Designation;

                        var objets = new List<ModeleObjet>();
                        if (curRsq.Objets != null && curRsq.Objets.Any()) {
                            curRsq.Objets.ForEach(o => {
                                objets.Add(new ModeleObjet {
                                    Code = o.Code.ToString(),
                                    Designation = o.Designation
                                });
                            });
                        }
                        model.ObjetsRisqueAll = new ModeleObjetsRisque { Objets = objets };
                    }
                }
                model.ClauseType = clauseType;
                model.CreateClause = createClause;

                model.DebutEffet = AlbConvert.ConvertIntToDate(model.DateDebut);
                if (model.DebutEffet.HasValue) {
                    var datedeb = model.DebutEffet.ToString();
                    var datedebsplit = datedeb.Split('/');
                    var datedebannee = datedebsplit[2].Split(' ')[0];
                    model.DateDeb = string.Format("{0}/{1}/{2}", datedebsplit[0], datedebsplit[1], datedebannee);
                }

                model.FinEffet = AlbConvert.ConvertIntToDate(model.DateFin);
                if (model.FinEffet != null) {
                    var datefin = model.FinEffet.ToString();
                    var datefinsplit = datefin.Split('/');
                    var datefinannee = datefinsplit[2].Split(' ')[0];
                    model.DateF = string.Format("{0}/{1}/{2}", datefinsplit[0], datefinsplit[1], datefinannee);
                }
                else
                    model.DateF = string.Empty;
                if (!string.IsNullOrEmpty(createClause)) {
                    var tParamCreate = createClause.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                    if (tParamCreate != null && tParamCreate.Length > 1) {
                        var tParam = tParamCreate[1].Split('_');
                        if (tParam != null && tParam.Length > 1) {
                            if (string.IsNullOrEmpty(model.Emplacement))
                                model.Emplacement = tParam[0];
                            if (string.IsNullOrEmpty(model.SousEmplacement))
                                model.SousEmplacement = tParam[1].ToUpper();
                            if (model.Ordre == 0)
                                model.Ordre = !string.IsNullOrEmpty(tParam[2]) ? Convert.ToInt32(tParam[2]) : 0;
                        }
                    }
                }
            }
            if (isReadonly) {
                model.Modifiable = false;
            }

            return PartialView("InfoClauseLibre", model);
        }

        [ErrorHandler]
        public string CheckSessionClause(int idSessionClause) {
            var userAD = System.Web.HttpContext.Current.User.Identity.Name.Trim().ToLower().Split(new[] { '\\' }, StringSplitOptions.None).LastOrDefault();
            var ip = Request.UserHostAddress;
#if DEBUG
            ip = AlbNetworkInfo.GetIpMachine();
#endif

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                var serviceContext = channelClient.Channel;
                var ret = serviceContext.CheckSessionClause(idSessionClause, ip, userAD);
                return ret;
            }
        }

        #endregion
    }
}
