using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRecherche;
using EmitMapper;
using Mapster;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Ecran.Rercherchesaisie;
using OP.WSAS400.DTO.Partenaire;
using OP.WSAS400.DTO.Personnes;
using OPServiceContract.IAdministration;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle
{
    public class AnCreationContratController : ControllersBase<AnCreationContratPage>
    {
        /// <summary>
        /// Gets a value indicating whether the page is readonly. Use EditMode to defines whether create mode is active
        /// </summary>
        public override bool IsReadonly => base.IsReadonly && this.model.EditMode;

        [ErrorHandler]
        public ActionResult Index(string id)
        {
            var match = Regex.Match(id, @"^(tabGuid(\w+)tabGuid)?modeNavigSmodeNavig$");
            if (match.Success)
            {
                model.AllParameters = AlbParameters.Parse(id);
                id = null;
            }

            if (id.ContainsChars())
            {
                id = InitializeParams(id);
                this.model.EditMode = true;
            }

            LoadContratInfo(id);
            SetNavigation(id);
            if (model.ContratInfoBase != null)
            {
                model.IsReadOnly = IsReadonly;
                model.InformationContrat.IsReadOnly = model.IsReadOnly;
                model.InformationContrat.IsModifHorsAvn = model.IsModifHorsAvenant = IsModifHorsAvenant;
                model.InformationBase.IsReadOnly = model.IsReadOnly;
                model.CourtierApporteur.EditMode = !model.IsReadOnly;
                model.CourtierGestionnairePayeur.CourtierGestionnaire.EditMode = !model.IsReadOnly || model.IsModifHorsAvenant;
                model.CourtierGestionnairePayeur.CourtierPayeur.EditMode = !model.IsReadOnly || model.IsModifHorsAvenant;
            }

            if (model.EditMode)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie");
            }
            else
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("InfoSaisie", returnEmptyTree: true); //new Models.ModelesNavigationArbre.ModeleNavigationArbre();
            }

            model.PageEnCours = NomsInternesEcran.CreationContrat.ToString();
            return View(model);
        }

        [ErrorHandler]
        public ActionResult GetCibles(string codeBranche)
        {
            List<AlbSelectListItem> listCibles = new List<AlbSelectListItem>();
            Cible_InformationBase model = new Cible_InformationBase();
            using (var client = ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetCibles(codeBranche, false, ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(
                el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                if (result != null)
                {
                    model.Cible = string.Empty;
                    model.Cibles = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                return PartialView("Cible_InfoBase", model);
            }
        }

        [ErrorHandler]
        public ActionResult GetTemplates(string codeBranche, string codeCible)
        {
            using (var chan = ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext = chan.Channel;
                var result = serviceContext.GetDefaultTemplateContrat(AlbConstantesMetiers.TYPE_CONTRAT, "0", codeCible, codeBranche, model.ModeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    return LoadDataTemplate(ObjectMapperManager.DefaultInstance.GetMapper<ContratInfoBaseDto, ContratInfoBaseDto>().Map(result), codeBranche, codeCible, result.CodeContrat);
                }
            }
            return null;
        }

        [ErrorHandler]
        public ActionResult GetTemplatePage(string codeBranche, string codeCible, string codeTemplate, string modeNavig)
        {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>())
            {
                var serviceContext = client.Channel;
                ContratInfoBaseDto contratInfoBase = null;
                //Gestion de la copie à partir d'un template choisi en amont
                if (codeTemplate.StartsWith("CV"))
                {
                    contratInfoBase = serviceContext.InitContratInfoBase(codeTemplate, "0", AlbConstantesMetiers.TYPE_CONTRAT, string.Empty, GetUser(), modeNavig.ParseCode<ModeConsultation>());
                }
                else
                {
                    contratInfoBase = serviceContext.InitContratInfoBase(null, null, null, string.Empty, GetUser(), modeNavig.ParseCode<ModeConsultation>());
                }
                return LoadDataTemplate(contratInfoBase, codeBranche, codeCible, codeTemplate);
            }
        }

        [AlbAjaxRedirect]
        [HandleJsonError]
        public RedirectToRouteResult UpdateContrat(AnInformationsBaseEnregister saveModel) {
            if (string.IsNullOrEmpty(saveModel.CodeContrat) && saveModel.Contrat != null) {
                saveModel.CodeContrat = saveModel.Contrat.CodeContrat;
                saveModel.VersionContrat = saveModel.Contrat.VersionContrat;
                saveModel.Type = saveModel.Contrat.Type;
            }

            if (saveModel.Type != AlbConstantesMetiers.TYPE_CONTRAT) {
                throw new AlbFoncException(string.Format("Erreur d'enregistrement du contrat n°", saveModel.CodeContrat.Trim()), true, true);
            }
            saveModel.TypeAvt = GetAddParamValue(saveModel.AddParamValue, AlbParameterName.AVNTYPE);
            saveModel.NumAvenant = GetAddParamValue(saveModel.AddParamValue, AlbParameterName.AVNID);

            if (saveModel.TypeContrat == "S" && (!string.IsNullOrEmpty(saveModel.NumAliment) || (!string.IsNullOrEmpty(saveModel.ContratMere)))) {
                saveModel.NumAliment = string.Empty;
                saveModel.ContratMere = string.Empty;
            }

            // Sauvegarde uniquement si l'écran n'est pas en readonly
            string codeRetour = saveModel.CodeContrat;
            if (!IsReadonly || IsModifHorsAvenant) {
                var contratInfoBase = saveModel.LoadContratInfoBaseDto();
                contratInfoBase.IsModifHorsAvn = IsModifHorsAvenant;
                string MsgStr = string.Empty;
                using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                    var serviceContext = client.Channel;

                    MsgStr = serviceContext.ControleSousGest(saveModel.SouscripteurCode, saveModel.GestionnaireCode);

                    if (!string.IsNullOrEmpty(MsgStr)) {
                        throw new AlbFoncException(MsgStr, false, false, true);
                    }

                    //VerificationPartenairesContrat(saveModel, serviceContext);
                    codeRetour = serviceContext.UpdateContrat(contratInfoBase, GetUser(), AlbConstantesMetiers.TRAITEMENT_AFFNV, GetUser());

                    if (codeRetour.Contains("MESSAGE : ")) {
                        throw new AlbFoncException(codeRetour.Replace("MESSAGE : ", ""), false, false, true);
                    }

                    if (!string.IsNullOrEmpty(contratInfoBase.NumAliment)) {
                        saveModel.VersionContrat = long.Parse(contratInfoBase.NumAliment);
                        this.model.VersionContrat = long.Parse(contratInfoBase.NumAliment);
                    }
                }
            }

            return Redirect(saveModel, codeRetour);
        }

        private RedirectToRouteResult Redirect(AnInformationsBaseEnregister saveModel, string newCode) {
            if (!string.IsNullOrEmpty(saveModel.txtParamRedirect)) {
                var tabParamRedirect = saveModel.txtParamRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            
            string id = AlbParameters.BuildStandardId(
                new Folder(new[] { newCode, saveModel.VersionContrat.ToString(), saveModel.Type }),
                saveModel.TabGuid,
                saveModel.AddParamValue,
                saveModel.ModeNavig);

            if (!IsReadonly && saveModel.CodeContrat.IsEmptyOrNull()) {
                InitVerrouillage(newCode, (int)this.model.VersionContrat);
            }

            if (!string.IsNullOrEmpty(saveModel.TypeAvt)) {
                return RedirectToAction("Index", "AvenantInfoGenerales", new { id, returnHome = saveModel.txtSaveCancel, guidTab = saveModel.TabGuid });
            }
            return RedirectToAction("Index", "AnInformationsGenerales", new { id, returnHome = saveModel.txtSaveCancel, guidTab = saveModel.TabGuid });
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string param)
        {
            return RedirectToAction(job, cible, new { id = param });
        }

        [ErrorHandler]
        public ActionResult LoadGestionnairePayeur(string codeGestionnaire, string nomGestionnaire, string nomInterlocuteur, string refInterlocuteur, int codeInterlocuteur,
              string codePayeur, string nomPayeur)
        {

            LoadCourtiers(codeGestionnaire, nomGestionnaire, nomInterlocuteur, refInterlocuteur, codeInterlocuteur, codePayeur, nomPayeur);
            return PartialView("DivFlottanteGestionnairePayeur", model.CourtierGestionnairePayeur);
        }

        [ErrorHandler]
        public ActionResult ValiderGestionnairePayeur(string codeGestionnaire, string nomGestionnaire, string nomInterlocuteur, string refInterlocuteur, int codeInterlocuteur,
               string codePayeur, string nomPayeur)
        {
            LoadCourtiers(codeGestionnaire, nomGestionnaire, nomInterlocuteur, refInterlocuteur, codeInterlocuteur, codePayeur, nomPayeur);
            return PartialView("CourtierGestionnairePayeur", model.CourtierGestionnairePayeur);
        }

        [ErrorHandler]
        public string GetNumeroAliment(string contratMere)
        {
            string toReturn = string.Empty;
            if (!string.IsNullOrEmpty(contratMere))
            {
                using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = client.Channel;
                    toReturn = serviceContext.GetNumeroAliment(contratMere);
                }
            }
            return toReturn;
        }

        [ErrorHandler]
        public ActionResult GetListeMotsCles(string codeBranche, string codeCible)
        {
            var toReturn = new InformationContrat();
            if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeCible))
            {
                toReturn.MotsClefs1 = LstMotsCles(codeBranche, codeCible);
                toReturn.MotsClefs2 = LstMotsCles(codeBranche, codeCible);
                toReturn.MotsClefs3 = LstMotsCles(codeBranche, codeCible);
            }
            else
            {
                toReturn.MotsClefs1 = new List<AlbSelectListItem>();
                toReturn.MotsClefs2 = new List<AlbSelectListItem>();
                toReturn.MotsClefs3 = new List<AlbSelectListItem>();
            }
            return PartialView("MotsCles", toReturn);
        }
        
        #region Méthodes privées
        private void SetNavigation(string id)
        {
            if (model.EditMode)
                model.PageTitle = "Informations de base";
            else
                model.PageTitle = "Création d'un contrat";
            model.AfficherNavigation = true;
            if (id != null && !string.IsNullOrEmpty(id))
                model.Navigation = new Navigation_MetaModel
                {
                    Etape = Navigation_MetaModel.ECRAN_INFOGENERALE
                };
            else model.Navigation = new Navigation_MetaModel
            {
                Etape = Navigation_MetaModel.ECRAN_SAISIE
            };

        }

        private void LoadContratInfo(string id) {
            bool modeCreationFromCV = false;
            bool modeEdition = false;
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                if (!string.IsNullOrEmpty(id) && id != null) {
                    if (this.model.AllParameters.TemplateId.GetValueOrDefault() > 0) {
                        model.ContratInfoBase = serviceContext.GetInfoTemplate(model.AllParameters.TemplateId.StringValue());
                        model.EditMode = false;
                        model.TemplateMode = true;
                    }
                    else {
                        string[] tId = id.Split('_');

                        if (tId.Length == 4) {
                            modeCreationFromCV = InitCopyContrat(serviceContext, tId);
                        }
                        else {
                            this.model.ContratInfoBase = serviceContext.InitContratInfoBase(tId[0], tId[1], tId[2], string.Empty, GetUser(), model.ModeNavig.ParseCode<ModeConsultation>());
                            this.model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                            if (this.model.ContratInfoBase == null) {
                                return;
                            }
                            modeEdition = true;
                            InitEditContrat(id, tId);
                        }
                    }
                }
                else {
                    InitNewContrat(serviceContext);
                }
                var branches = model.ContratInfoBase.Branches;
                branches = branches.Where(el => CacheUserRights.UserRights.Any(elU => (elU.Branche == el.Code || elU.Branche == "**") && elU.TypeDroit != TypeDroit.V.ToString())).ToList();
                model.ContratInfoBase.Branches = branches;

                LoadDataContrat();
                if (modeEdition) {
                    model.InformationBase.InformationTemplate = null;
                }
                if (modeCreationFromCV) {
                    InitDataTemplate();
                }
            }
        }

        /// <summary>
        /// Initializes new Contrat data.
        /// </summary>
        /// <param name="serviceContext"></param>
        private void InitNewContrat(IAffaireNouvelle serviceContext) {
            this.model.ContratInfoBase = serviceContext.InitContratInfoBase(null, null, null, string.Empty, GetUser(), model.ModeNavig.ParseCode<ModeConsultation>());
            this.model.ContratInfoBase.Type = AlbConstantesMetiers.TYPE_CONTRAT;
            this.model.Type = AlbConstantesMetiers.TYPE_CONTRAT;
            this.model.VersionContrat = 0;
        }

        private void InitEditContrat(string id, string[] tId) {
            model.CodeContrat = tId[0];
            model.Type = tId[2];
            if (long.TryParse(tId[1], out long version)) {
                model.VersionContrat = version;
            }
            else {
                model.VersionContrat = 0;
            }
            if (model.CodeContrat.Contains("CV")) {
                model.TemplateMode = true;
            }
            model.Bandeau = null;
            model.AfficherBandeau = base.DisplayBandeau(true, id);
            model.AfficherNavigation = model.AfficherBandeau;
            model.AfficherArbre = true;
            if (model.AfficherBandeau) {
                model.Bandeau = base.GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
            }
            string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            switch (typeAvt) {
                case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    break;
                case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                    model.Bandeau.StyleBandeau = model.ScreenType;
                    break;

                case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                    break;
                default:
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    break;
            }
        }

        /// <summary>
        /// Initialize new Contrat data for copy from a Contrat or a Canevas
        /// </summary>
        /// <param name="modeCreationFromCV"></param>
        /// <param name="serviceContext"></param>
        /// <param name="tId"></param>
        /// <returns>A value indicating whether the new contrat is created form a Canevas</returns>
        private bool InitCopyContrat(IAffaireNouvelle serviceContext, string[] tId) {
            var resultBC = serviceContext.InitContratInfoBase(tId[0], tId[1], tId[2], string.Empty, GetUser(), model.ModeNavig.ParseCode<ModeConsultation>());
            string branche = resultBC.Branche;
            string cible = resultBC.Cible;
            bool modeCreationFromCV = false;
            if (tId[0].StartsWith("CV"))
            {
                model.ContratInfoBase = resultBC;
                modeCreationFromCV = true;
            }
            else {
                model.ContratInfoBase = serviceContext.InitContratInfoBase(null, null, null, string.Empty, GetUser(), model.ModeNavig.ParseCode<ModeConsultation>());
                model.ContratInfoBase.CodeMotsClef1 = resultBC.CodeMotsClef1;
                model.ContratInfoBase.CodeMotsClef2 = resultBC.CodeMotsClef2;
                model.ContratInfoBase.CodeMotsClef3 = resultBC.CodeMotsClef3;
            }
            model.ContratInfoBase.Branche = branche;
            model.ContratInfoBase.Cible = cible;
            model.ContratInfoBase.Type = AlbConstantesMetiers.TYPE_CONTRAT;
            model.Type = AlbConstantesMetiers.TYPE_CONTRAT;
            model.VersionContrat = 0;
            model.CopyMode = true;
            model.EditMode = false;
            model.CodeContratCopy = tId[0];
            model.VersionCopy = tId[1];

            // Si le contrat en paramètre est un contrat Mère
            if (resultBC.TypePolice == "M") {
                model.ContratInfoBase.TypePolice = resultBC.TypePolice;
            }
            model.ContratInfoBase.DateAccordAnnee = 0;
            model.ContratInfoBase.DateAccordMois = 0;
            model.ContratInfoBase.DateAccordJour = 0;
            model.ContratInfoBase.SouscripteurCode = string.Empty;
            model.ContratInfoBase.SouscripteurNom = string.Empty;
            model.ContratInfoBase.GestionnaireCode = string.Empty;
            model.ContratInfoBase.GestionnaireNom = string.Empty;
            model.ContratInfoBase.CourtierGestionnaire = 0;
            model.ContratInfoBase.NomCourtierGest = string.Empty;
            model.ContratInfoBase.CourtierApporteur = 0;
            model.ContratInfoBase.NomCourtierAppo = string.Empty;
            model.ContratInfoBase.CourtierPayeur = 0;
            model.ContratInfoBase.NomCourtierPayeur = string.Empty;
            model.ContratInfoBase.CodePreneurAssurance = 0;
            model.ContratInfoBase.NomPreneurAssurance = string.Empty;
            model.ContratInfoBase.AdresseContrat = null;
            return modeCreationFromCV;
        }

        private void SetInfoBase(ContratInfoBaseDto contratInfoBaseDto)
        {
            var branches = new List<AlbSelectListItem>();
            contratInfoBaseDto.Branches.ToList().ForEach(
                elem => branches.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            var typesContrat = new List<AlbSelectListItem>();
            contratInfoBaseDto.TypesContrat.ToList().ForEach(
                elem => typesContrat.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );

            var model = new InformationBase
            {
                Branche = contratInfoBaseDto.Branche,
                Branches = branches,
                Cible_InformationBase = GetCibleModel(contratInfoBaseDto.Branche, contratInfoBaseDto.Cible),
                TypeContrat = !string.IsNullOrEmpty(contratInfoBaseDto.TypePolice) ? contratInfoBaseDto.TypePolice : "S",
                TypesContrat = typesContrat,
                ContratMere = contratInfoBaseDto.ContratMere,
                ContratRemplace = contratInfoBaseDto.ContratRemplace,
                NumAliment = contratInfoBaseDto.NumAliment,
                NumAlimentRemplace = contratInfoBaseDto.NumAlimentRemplace,
                NumContratRemplace = contratInfoBaseDto.NumContratRemplace,
                EditMode = base.model.EditMode,
                TemplateMode = base.model.TemplateMode,
                CopyMode = base.model.CopyMode,
                Descriptif = contratInfoBaseDto.Descriptif
            };

            if (contratInfoBaseDto.DateAccordAnnee != 0 && contratInfoBaseDto.DateAccordMois != 0 && contratInfoBaseDto.DateAccordJour != 0)
                model.DateAccord = new DateTime(contratInfoBaseDto.DateAccordAnnee, contratInfoBaseDto.DateAccordMois, contratInfoBaseDto.DateAccordJour);

            if (!string.IsNullOrEmpty(contratInfoBaseDto.SouscripteurCode))
            {
                model.SouscripteurCode = contratInfoBaseDto.SouscripteurCode;
                model.SouscripteurNom = contratInfoBaseDto.SouscripteurCode + " - " + contratInfoBaseDto.SouscripteurNom;
            }

            if (!string.IsNullOrEmpty(contratInfoBaseDto.GestionnaireCode))
            {
                model.GestionnaireCode = contratInfoBaseDto.GestionnaireCode;
                model.GestionnaireNom = contratInfoBaseDto.GestionnaireCode + " - " + contratInfoBaseDto.GestionnaireNom;
            }
            else
            {
                using (var client = ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var screenClient = client.Channel;
                    var res = screenClient.GestionnairesGet(new GestionnairesGetQueryDto { NomGestionnaire = GetUser(), DebutPagination = 1, FinPagination = 1 });
                    if (res.GestionnairesDto.Any())
                    {
                        model.GestionnaireCode = res.GestionnairesDto.FirstOrDefault().Id;
                        model.GestionnaireNom = res.GestionnairesDto.FirstOrDefault().Id + " - " + res.GestionnairesDto.FirstOrDefault().Nom;
                    }
                }
            }

            if (!string.IsNullOrEmpty(contratInfoBaseDto.SouscripteurCode))
            {
                model.SouscripteurCode = contratInfoBaseDto.SouscripteurCode;
                model.SouscripteurNom = contratInfoBaseDto.SouscripteurCode + " - " + contratInfoBaseDto.SouscripteurNom;
            }
            else
            {
                using (var client = ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var screenClient = client.Channel;
                    var res = screenClient.SouscripteursGet(new SouscripteursGetQueryDto { NomSouscripteur = GetUser(), DebutPagination = 1, FinPagination = 1 });
                    if (res.SouscripteursDto.Any())
                    {
                        model.SouscripteurCode = res.SouscripteursDto.FirstOrDefault().Code;
                        model.SouscripteurNom = res.SouscripteursDto.FirstOrDefault().Code + " - " + res.SouscripteursDto.FirstOrDefault().Nom;
                    }
                }
            }

            model.InformationTemplate = new Template_InformationBase
            {
                Template = !string.IsNullOrEmpty(base.model.CodeContratCopy) && (base.model.CodeContratCopy.Contains("CV")) ? base.model.CodeContratCopy : string.Empty,
                Templates = GetListeTemplates(model.Branche, model.Cible_InformationBase.Cible),
                CopyMode = base.model.CopyMode,
                TemplateMode = base.model.TemplateMode
            };

            if (model.InformationTemplate.Templates.Find(elm => elm.Value.Trim() == (string.IsNullOrEmpty(model.InformationTemplate.Template) ? string.Empty : model.InformationTemplate.Template.Trim())) != null)
                model.InformationTemplate.Templates.Find(elm => elm.Value.Trim() == model.InformationTemplate.Template.Trim()).Selected = true;

            if (base.model.EditMode)
            {
                model.Descriptif = base.model.ContratInfoBase.Descriptif;

                if (contratInfoBaseDto.DateEffetAnnee != 0 && contratInfoBaseDto.DateEffetMois != 0 && contratInfoBaseDto.DateEffetJour != 0)
                    model.DateEffet = new DateTime(contratInfoBaseDto.DateEffetAnnee, contratInfoBaseDto.DateEffetMois, contratInfoBaseDto.DateEffetJour);

                model.HeureEffet = AlbConvert.ConvertIntToTimeMinute(contratInfoBaseDto.DateEffetHeure);


            }

            //Si le contrat en paramètre est un contrat Mère
            if (contratInfoBaseDto.TypePolice == "M")
            {
                //TODO : à vérifier avec SLA Correction dûe au bug 1193 corrigé le 2014-12-31 
                model.TypeContrat = "M";
                //model.TypeContrat = "A";
                model.ContratMereParam = base.model.CodeContratCopy;
            }
            else if (contratInfoBaseDto.TypePolice == "A")
            {
                model.NumAliment = base.model.VersionContrat.ToString();
                model.ContratMere = base.model.CodeContrat;
            }

            base.model.InformationBase = model;
        }
        private Cible_InformationBase GetCibleModel(string codeBranche, string codeCible)
        {
            List<AlbSelectListItem> listCibles = new List<AlbSelectListItem>();
            var model = new Cible_InformationBase();
            using (var client = ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.GetCibles(codeBranche, false, CacheUserRights.UserRights.Any(
                    el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                if (result != null)
                {
                    model.Cible = codeCible;
                    model.Cibles = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                }
                model.EditMode = base.model.EditMode;
                model.TemplateMode = base.model.TemplateMode;
                model.CopyMode = base.model.CopyMode;
                return model;
            }
        }

        private void SetCourtierApporteur(ContratInfoBaseDto contratInfoBaseDto)
        {
            var model = new CourtierApporteur
            {
                Courtier = new Courtier
                {
                    CodeCourtier = contratInfoBaseDto.CourtierApporteur != 0 ? contratInfoBaseDto.CourtierApporteur.ToString() : string.Empty,
                    NomCourtier = contratInfoBaseDto.NomCourtierAppo
                },
                EditMode = base.model.EditMode
            };

            base.model.CourtierApporteur = model;
        }

        private void SetGestionnairePayeur(ContratInfoBaseDto contratInfoBaseDto)
        {
            var encaissements = new List<AlbSelectListItem>();
            model.ContratInfoBase.Encaissements.ToList().ForEach(
                 elem => encaissements.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            //Gestionnaire
            var modelGestionnaire = new Courtier
            {
                CodeCourtier = contratInfoBaseDto.CourtierGestionnaire != 0 ? contratInfoBaseDto.CourtierGestionnaire.ToString() : string.Empty,
                NomCourtier = contratInfoBaseDto.NomCourtierGest,
                Reference = contratInfoBaseDto.RefCourtier,
                Encaissement = contratInfoBaseDto.Encaissement,
                Encaissements = encaissements,
                IsReadOnly = model.IsReadOnly
            };

            if (contratInfoBaseDto.CodeInterlocuteur != 0)
            {
                modelGestionnaire.CodeInterlocuteur = contratInfoBaseDto.CodeInterlocuteur;
                modelGestionnaire.NomInterlocuteur = contratInfoBaseDto.NomInterlocuteur;
            }
            //Payeur

            var modelPayeur = new Courtier
            {
                CodeCourtier = contratInfoBaseDto.CourtierPayeur != 0 ? contratInfoBaseDto.CourtierPayeur.ToString() : string.Empty,
                NomCourtier = contratInfoBaseDto.NomCourtierPayeur

            };

            model.CourtierGestionnairePayeur = new CourtierGestionnairePayeur
            {
                CourtierGestionnaire = modelGestionnaire,
                CourtierPayeur = modelPayeur
                //Encaissement = contratInfoBaseDto.Encaissement,
                //Encaissements = encaissements
            };
        }

        private void SetPreneurAssurance(ContratInfoBaseDto contratInfoBaseDto)
        {
            var model = new PreneurAssurance
            {
                Numero = contratInfoBaseDto.CodePreneurAssurance != 0 ? contratInfoBaseDto.CodePreneurAssurance.ToString() : string.Empty,
                Nom = contratInfoBaseDto.NomPreneurAssurance,
                Departement = contratInfoBaseDto.DepAssurance,
                Ville = contratInfoBaseDto.VilleAssurance,
                PreneurEstAssure = contratInfoBaseDto.PreneurEstAssure,
                NbAssuAdditionnel = base.model.Contrat != null ? base.model.Contrat.NbAssuresAdditionnels : 0
            };

            base.model.PreneurAssurance = model;
        }

        private void SetInformationContrat(ContratInfoBaseDto contratInfoBaseDto)
        {
            //var motscles1 = new List<AlbSelectListItem>();
            //var motscles2 = new List<AlbSelectListItem>();
            //var motscles3 = new List<AlbSelectListItem>();

            //using (var screenClient = new SaisieOffreClient())
            //{
            //    var query = new ModifierOffreGetQueryDto();
            //    var result = screenClient.ModifierOffreGet(query, contratInfoBaseDto.Branche, contratInfoBaseDto.Cible);
            //    motscles1 = result.MotsCles.Select(
            //               m => new AlbSelectListItem
            //               {
            //                   Value = m.Code,
            //                   Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
            //                   Selected = false,
            //                   Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
            //               }).ToList();
            //    motscles2 = result.MotsCles.Select(
            //               m => new AlbSelectListItem
            //               {
            //                   Value = m.Code,
            //                   Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
            //                   Selected = false,
            //                   Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
            //               }).ToList();
            //    motscles3 = result.MotsCles.Select(
            //               m => new AlbSelectListItem
            //               {
            //                   Value = m.Code,
            //                   Text = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : "",
            //                   Selected = false,
            //                   Title = !string.IsNullOrEmpty(m.Code) || !string.IsNullOrEmpty(m.Libelle) ? string.Format("{0} - {1}", m.Code, m.Libelle) : ""
            //               }).ToList();
            //}
            var model = new InformationContrat
            {
                MotClef1 = contratInfoBaseDto.CodeMotsClef1,
                MotClef2 = contratInfoBaseDto.CodeMotsClef2,
                MotClef3 = contratInfoBaseDto.CodeMotsClef3,
                Observation = contratInfoBaseDto.Obersvations,
                NumChronoObsv = contratInfoBaseDto.NumChronoObsv.ToString()
            };



            base.model.InformationContrat = model;

        }

        private void SetAdresseRisque(ContratInfoBaseDto contratInfoBaseDto)
        {
            //var model = new ContactAddresse_MetaData();
            var model = new ModeleContactAdresse();
            model.SaisieHexavia = true;
            //model.ReadOnly = true;
            model.ReadOnly = false;
            if (contratInfoBaseDto != null)
            {
                var tabGuid = string.Format("tabGuid{0}tabGuid", base.model.TabGuid);
                var folder = string.Format("{0}_{1}_{2}", contratInfoBaseDto.CodeContrat, contratInfoBaseDto.VersionContrat.ToString(CultureInfo.InvariantCulture), contratInfoBaseDto.Type);

                model.ReadOnly = GetIsReadOnly(tabGuid, folder);
                model.IsModifHorsAvn = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(contratInfoBaseDto.NumAvenant) ? "0" : contratInfoBaseDto.NumAvenant));
            }
            if (contratInfoBaseDto.AdresseContrat != null)
            {
                int depart = 0;
                Int32.TryParse(contratInfoBaseDto.AdresseContrat.Departement, out depart);
                string codePostal = string.Empty;
                string codePX = string.Empty;
                //string codePostal = depart.ToString("D2") + currentObj.AdresseObjet.CodePostal.ToString("D3");
                //string codePX = depart.ToString("D2") + currentObj.AdresseObjet.CodePostalCedex.ToString("D3");
                if (depart > 0)
                {
                    codePostal = depart.ToString("D2") + contratInfoBaseDto.AdresseContrat.CodePostal.ToString("D3");
                    codePX = depart.ToString("D2") + contratInfoBaseDto.AdresseContrat.CodePostalCedex.ToString("D3");
                }
                else
                {
                    codePostal = contratInfoBaseDto.AdresseContrat.CodePostal.ToString();
                    codePX = contratInfoBaseDto.AdresseContrat.CodePostalCedex.ToString();
                }

                model.Batiment = contratInfoBaseDto.AdresseContrat.Batiment;
                model.Distribution = contratInfoBaseDto.AdresseContrat.BoitePostale;
                model.CodePostal = codePostal;
                model.CodePostalCedex = codePX;
                model.Extension = contratInfoBaseDto.AdresseContrat.ExtensionVoie;
                model.Voie = contratInfoBaseDto.AdresseContrat.NomVoie;
                model.No = contratInfoBaseDto.AdresseContrat.NumeroVoie == 0 ? string.Empty : contratInfoBaseDto.AdresseContrat.NumeroVoie.ToString();
                model.No2 = contratInfoBaseDto.AdresseContrat.NumeroVoie2;
                model.NoChrono = contratInfoBaseDto.AdresseContrat.NumeroChrono;
                model.Pays = contratInfoBaseDto.AdresseContrat.NomPays;
                model.Ville = contratInfoBaseDto.AdresseContrat.NomVille;
                model.VilleCedex = contratInfoBaseDto.AdresseContrat.NomCedex;
                model.Latitude = contratInfoBaseDto.AdresseContrat.Latitude?.ToString();
                model.Longitude = contratInfoBaseDto.AdresseContrat.Longitude?.ToString();
                model.MatriculeHexavia = contratInfoBaseDto.AdresseContrat.MatriculeHexavia;
                //if (!string.IsNullOrEmpty(model.CodePostal) && !(string.IsNullOrEmpty(contratInfoBaseDto.AdresseContrat.Departement)))
                //    model.CodePostal = contratInfoBaseDto.AdresseContrat.Departement + model.CodePostal;
                //if (!string.IsNullOrEmpty(model.CodePostalCedex) && !(string.IsNullOrEmpty(contratInfoBaseDto.AdresseContrat.Departement)))
                //    model.CodePostalCedex = contratInfoBaseDto.AdresseContrat.Departement + model.CodePostalCedex;
            }
            base.model.ContactAdresse = model;
        }

        private void SetRechercheSaisie(AnCreationContratPage contentData)
        {
            contentData.Recherche = new ModeleRecherchePage
            {
                CritereParam = AlbConstantesMetiers.CriterParam.ContratOnly,
                ProvenanceParam = "connexite",
                SituationParam = string.Empty,
                ListCabinetCourtage = new ModeleRechercheAvancee(),
                ListPreneurAssurance = new ModeleRechercheAvancee()
            };
            using (var client = ServiceClientFactory.GetClient<IRechercheSaisie>())
            {
                var screenClient = client.Channel;
                var query = new RechercheSaisieGetQueryDto
                {
                    IsAdmin = ALBINGIA.OP.OP_MVC.Common.CacheUserRights.UserRights.Any(
                        el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()),
                    IsUserHorse = MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0
                };
                var result = screenClient.RechercheSaisieGet(query);
                if (result != null)
                {
                    var cibles = result.Cibles.Select(m => new AlbSelectListItem() { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.ModeleCibles = new ModeleListeCibles();
                    contentData.Recherche.ModeleCibles.Cibles = cibles;
                    var branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.Branches = branches;
                    var etats = result.Etats.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.Etats = etats;
                    var situations = result.Situation.Select(m => new AlbSelectListItem
                    {
                        Value = m.Code,
                        Text = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty,
                        Selected = false,
                        Title = !string.IsNullOrEmpty(m.Code) ? string.Format("{0} - {1}", m.Code, m.Libelle) : string.Empty
                    }).ToList();
                    contentData.Recherche.Situations = situations;
                    var listRefus = result.ListRefus.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    contentData.Recherche.ListRefus = listRefus;
                    contentData.Recherche.DateTypes = AlbTransverse.InitDateType;
                }
            }
        }

        private void LoadDataContrat()
        {
            SetInfoBase(model.ContratInfoBase);
            SetCourtierApporteur(model.ContratInfoBase);
            #region Gestionnaire et Payeur
            SetGestionnairePayeur(model.ContratInfoBase);
            #region Encaissement
            var encaissements = new List<AlbSelectListItem>();
            model.ContratInfoBase.Encaissements.ToList().ForEach(
                 elem => encaissements.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );
            model.Encaissements = encaissements;
            model.Encaissement = model.ContratInfoBase.Encaissement;
            #endregion
            #endregion
            if ((model.CourtierGestionnairePayeur.CourtierGestionnaire.CodeCourtier == model.CourtierGestionnairePayeur.CourtierPayeur.CodeCourtier) && (model.CourtierApporteur.Courtier.CodeCourtier == model.CourtierGestionnairePayeur.CourtierGestionnaire.CodeCourtier))
            {
                model.GpIdentiqueApporteur = true;
                model.CourtierApporteur.IdentiqueChecked = true;
                model.CourtierGestionnairePayeur.CourtierGestionnaire.IdentiqueChecked = true;
                model.CourtierGestionnairePayeur.CourtierPayeur.IdentiqueChecked = true;
            }
            #region Preneur Assurance
            SetPreneurAssurance(model.ContratInfoBase);
            #endregion
            #region Inormation Contrat
            SetInformationContrat(model.ContratInfoBase);
            LoadListesMotsCles(model.ContratInfoBase.Branche, model.ContratInfoBase.Cible);
            #endregion
            #region Adresse risque
            SetAdresseRisque(model.ContratInfoBase);
            #endregion
            #region chargement de la recherche saisie
            SetRechercheSaisie(model);
            #endregion
            #region recherche avancée courtiers
            model.ListCabinetCourtageApporteur = new Models.ModelesRecherche.ModeleRechercheAvancee { Contexte = "courtierApporteur" };
            model.ListCabinetCourtageGestion = new Models.ModelesRecherche.ModeleRechercheAvancee { Contexte = "courtierGestion" };
            model.ListCabinetCourtagePayeur = new Models.ModelesRecherche.ModeleRechercheAvancee { Contexte = "courtierPayeur" };
            #endregion

            model.InformationBase.CopyMode = model.CopyMode;
            if (model.InformationBase.Cible_InformationBase != null)
                model.InformationBase.Cible_InformationBase.CopyMode = model.CopyMode;
            if (model.InformationBase.InformationTemplate != null)
                model.InformationBase.InformationTemplate.CopyMode = model.CopyMode;
        }

        private void LoadCourtiers(string codeGestionnaire, string nomGestionnaire, string nomInterlocuteur, string refInterlocuteur, int codeInterlocuteur, string codePayeur, string nomPayeur)
        {
            //Gestionnaire
            var modelGestionnaire = new Courtier
            {
                CodeCourtier = codeGestionnaire,
                NomCourtier = !string.IsNullOrEmpty(codeGestionnaire) ? nomGestionnaire : string.Empty,
                Reference = refInterlocuteur
            };

            if (codeInterlocuteur != 0)
            {
                modelGestionnaire.CodeInterlocuteur = codeInterlocuteur;
                modelGestionnaire.NomInterlocuteur = nomInterlocuteur;
            }
            //Payeur     

            var modelPayeur = new Courtier
            {
                CodeCourtier = codePayeur,
                NomCourtier = !string.IsNullOrEmpty(codePayeur) ? nomPayeur : string.Empty

            };

            //var encaissements = new List<AlbSelectListItem>();
            //using (var serviceContext = new AffaireNouvelleClient())
            //{
            //    serviceContext.GetEncaissements().ToList().ForEach(
            //         elem => encaissements.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            //    );
            //}
            model.CourtierGestionnairePayeur = new CourtierGestionnairePayeur
            {
                CourtierGestionnaire = modelGestionnaire,
                CourtierPayeur = modelPayeur
                //Encaissement = encaissement,
                //Encaissements = encaissements

            };
        }


        #region gestion des templates

        private List<AlbSelectListItem> GetListeTemplates(string codeBranche, string codeCible)
        {
            List<AlbSelectListItem> toReturn = null;
            if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeCible))
            {
                using (var chan = ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = chan.Channel;
                    var result = serviceContext.LoadListTemplates(0, string.Empty, string.Empty, AlbConstantesMetiers.TYPE_CONTRAT, codeCible, codeBranche, false, true);
                    if (result != null)
                    {
                        toReturn = result.Select(m => new AlbSelectListItem { Value = m.CodeTemplate, Text = string.Format("{0} - {1}", m.CodeTemplate, m.DescriptionTemplate), Selected = m.DefaultTemplate == "O", Title = string.Format("{0} - {1}", m.CodeTemplate, m.DescriptionTemplate) }).ToList(); ;
                    }
                }
            }
            if (toReturn == null)
                toReturn = new List<AlbSelectListItem>();
            return toReturn;
        }

        private ActionResult LoadDataTemplate(ContratInfoBaseDto template, string codeBranche, string codeCible, string codeTemplate)
        {
            model.ContratInfoBase = template;

            model.ContratInfoBase.Branche = codeBranche;
            model.ContratInfoBase.Cible = codeCible;
            model.ContratInfoBase.Type = AlbConstantesMetiers.TYPE_CONTRAT;
            model.Type = AlbConstantesMetiers.TYPE_CONTRAT;
            model.VersionContrat = 0;
            model.CopyMode = true;
            model.EditMode = false;
            model.CodeContratCopy = codeTemplate;
            model.VersionCopy = "0";

            LoadDataContrat();

            InitDataTemplate();

            if (string.IsNullOrEmpty(codeTemplate))
            {
                model.InformationBase.InformationTemplate.Templates.ForEach(elm => elm.Selected = false);
                model.CopyMode = false;
            }

            return PartialView("BodyAnCreationContrat", model);
        }

        #endregion

        private void InitDataTemplate()
        {
            model.LoadTemplateMode = true;
            model.InformationBase.DateAccord = null;
            var encaissements = new List<AlbSelectListItem>();
            model.ContratInfoBase.Encaissements.ToList().ForEach(
                 elem => encaissements.Add(AlbSelectListItem.ConvertToAlbSelect(elem.Code, elem.Libelle, elem.Libelle))
            );

            using (var client = ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = client.Channel;
                var res = screenClient.GestionnairesGet(new GestionnairesGetQueryDto { NomGestionnaire = GetUser(), DebutPagination = 1, FinPagination = 1 });
                if (res.GestionnairesDto.Any())
                {
                    model.InformationBase.GestionnaireCode = res.GestionnairesDto.FirstOrDefault().Id;
                    model.InformationBase.GestionnaireNom = res.GestionnairesDto.FirstOrDefault().Id + " - " + res.GestionnairesDto.FirstOrDefault().Nom;
                }
                else
                {
                    model.InformationBase.GestionnaireCode = string.Empty;
                    model.InformationBase.GestionnaireNom = string.Empty;
                }
                var sous = screenClient.SouscripteursGet(new SouscripteursGetQueryDto { NomSouscripteur = GetUser(), DebutPagination = 1, FinPagination = 1 });
                if (sous.SouscripteursDto.Any())
                {
                    model.InformationBase.SouscripteurCode = sous.SouscripteursDto.FirstOrDefault().Code;
                    model.InformationBase.SouscripteurNom = sous.SouscripteursDto.FirstOrDefault().Code + " - " + sous.SouscripteursDto.FirstOrDefault().Nom;
                }
                else
                {
                    model.InformationBase.SouscripteurCode = string.Empty;
                    model.InformationBase.SouscripteurNom = string.Empty;
                }
            }


            model.InformationBase.LoadTemplateMode = true;
            model.InformationBase.Cible_InformationBase.LoadTemplateMode = true;
            model.InformationBase.InformationTemplate.LoadTemplateMode = true;
            model.InformationBase.Cible_InformationBase.CopyMode = false;

            model.InformationBase.Descriptif = string.Empty;
            model.ContactAdresse = new ModeleContactAdresse();
        }

        private void LoadListesMotsCles(string codeBranche, string codeCible)
        {
            if (model.InformationContrat != null && !string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeCible))
            {
                model.InformationContrat.MotsClefs1 = LstMotsCles(codeBranche, codeCible);
                model.InformationContrat.MotsClefs2 = LstMotsCles(codeBranche, codeCible);
                model.InformationContrat.MotsClefs3 = LstMotsCles(codeBranche, codeCible);
            }
            else
            {
                model.InformationContrat.MotsClefs1 = new List<AlbSelectListItem>();
                model.InformationContrat.MotsClefs2 = new List<AlbSelectListItem>();
                model.InformationContrat.MotsClefs3 = new List<AlbSelectListItem>();
            }
        }

        private List<AlbSelectListItem> LstMotsCles(string codeBranche, string codeCible)
        {
            var value = new List<AlbSelectListItem>();
            using (var client = ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = client.Channel;
                var result = serviceContext.ObtenirMotClef(codeBranche, codeCible);
                if (result.Any())
                {
                    result.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code,
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }));
                }

            }
            return value;
        }
        /// <summary>
        /// Vérification des partenaires / contrat 
        /// </summary>
        /// <param name="contratInfoBase">contrat</param>
        /// <param name="numAvenant">AVN</param>
        private void VerificationPartenairesContrat(AnInformationsBaseEnregister model, IAffaireNouvelle serviceContext)
        {

            if (model != null && CanUseLablat())
            {

                var partenaires = new PartenairesDto
                {

                    CourtierGestionnaire = new PartenaireDto
                    {
                        Code = model.CodeCourtierGestionnaire.ToString(),
                        Nom = model.NomCourtierGestionnaire,
                        CodeInterlocuteur = model.CodeInterlocuteurGestionnaire,
                        NomInterlocuteur = model.NomInterlocuteurGestionnaire
                    },
                    CourtierApporteur = new PartenaireDto
                    {
                        Code = model.CodeCourtierApporteur.ToString(),
                        Nom = model.NomCourtierApporteur,

                    },
                    CourtierPayeur = new PartenaireDto
                    {
                        Code = model.CodeCourtierPayeur.ToString(),
                        Nom = model.NomCourtierPayeur,

                    },

                    PreneurAssurance = new PartenaireDto
                    {
                        Code = model.NumeroAssur.ToString(),
                        Nom = model.NomAssur
                    },
                    AssuresAdditionnels = serviceContext.GetListAssuresAdditionnelsInfosBase(
                                                         model.CodeContrat.Trim(),
                                                        model.VersionContrat.ToString(CultureInfo.InvariantCulture),
                                                         model.Type,
                                                        string.IsNullOrEmpty(model.NumAvenant) ? "0" : model.NumAvenant,
                                                         model.ModeNavig.ParseCode<ModeConsultation>()
                                                        )


                };
                VerificationPartenaires(partenaires);


            }
        }

        #endregion
    }
}
