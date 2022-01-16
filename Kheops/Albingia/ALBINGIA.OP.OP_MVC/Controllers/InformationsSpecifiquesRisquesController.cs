using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.DynamicGuiIS.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using Mapster;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres.Risque;
using OPServiceContract;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    public class InformationsSpecifiquesRisquesController : InformationsSpecifiquesControllerBase<ModeleInformationsSpecifiquesRisquesPage>
    {
        private static string _branche;
        private static string _cible;
        private static List<AlbSelectListItem> _lstTypesRegularisation;
        public static List<AlbSelectListItem> LstTypesRegularisation
        {
            get
            {
                //Si les données ont déjà été chargé en mémoire, on renvoit une nouvelle instance de la liste
                if (_lstTypesRegularisation != null)
                {
                    var toReturn = new List<AlbSelectListItem>();
                    _lstTypesRegularisation.ForEach(elm => toReturn.Add(new AlbSelectListItem
                    {
                        Value = elm.Value,
                        Text = elm.Text,
                        Title = elm.Title,
                        Selected = false
                    }));
                    return toReturn;
                }

                //Sinon on charge les données à partir de la BDD et on renvoit une nouvelle instance de la liste
                List<AlbSelectListItem> value = new List<AlbSelectListItem>();
                using (var channelClient = ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = channelClient.Channel;
                    var lstTypesRegularisation = serviceContext.GetListeTypesRegularisation(_branche, _cible);
                    lstTypesRegularisation.ForEach(elm => value.Add(new AlbSelectListItem
                    {
                        Value = elm.Code.ToString(),
                        Text = elm.Code + " - " + elm.Libelle,
                        Title = elm.Code + " - " + elm.Libelle,
                        Selected = false
                    }
                    ));
                }
                _lstTypesRegularisation = value;
                return value;
            }
        }
        public static List<AlbSelectListItem> LstTypesDest
        {
            get
            {
                var lstTypesDest = new List<AlbSelectListItem>
                {
                    new AlbSelectListItem
                    {
                        Value = "AS",
                        Text = "Assurés",
                        Title = "Assurés",
                        Descriptif = "Assurés"
                    },
                    new AlbSelectListItem
                    {
                        Value = "IN",
                        Text = "Intervenants",
                        Title = "Intervenants",
                        Descriptif = "Intervenants"
                    }
                };
                return lstTypesDest;
            }
        }
        public static List<AlbSelectListItem> LstPBBNS(string partBenefGenerale)
        {

            var toReturn = new List<AlbSelectListItem>();
            toReturn.Add(new AlbSelectListItem
            {
                Text = "",
                Value = "N",
                Title = "",
                Selected = false
            });
            if (string.IsNullOrEmpty(partBenefGenerale) || partBenefGenerale == "N" || partBenefGenerale == "O")
            {
                toReturn.Add(new AlbSelectListItem
                {
                    Text = "PB",
                    Value = "O",
                    Title = "Part Bénéficiaire",
                    Selected = false
                });
            }
            if (string.IsNullOrEmpty(partBenefGenerale) || partBenefGenerale == "N" || partBenefGenerale == "B")
            {
                toReturn.Add(new AlbSelectListItem
                {
                    Text = "BNS",
                    Value = "B",
                    Title = "Bonification",
                    Selected = false
                });
            }

            if (AlbOpConstants.ClientWorkEnvironment != AlbOpConstants.OPENV_PROD && (string.IsNullOrEmpty(partBenefGenerale) || partBenefGenerale == "N" || partBenefGenerale == "U"))
            {
                toReturn.Add(new AlbSelectListItem
                {
                    Text = "BURNER",
                    Value = "U",
                    Title = "Burner",
                    Selected = false
                });
            }
            return toReturn;
        }

        
        [ErrorHandler]
        public ActionResult Index(string id)
        {
            id = InitializeParams(id);
            model.NomEcran = NomsInternesEcran.InformationsSpecifiquesRisque;
            LoadInfoPage(id);
            return View(model);
        }

        [ErrorHandler]
        public ActionResult OpenInfoSpecifiquesRisque(string id, string IsISLoaded)
        {
            string formattedId = InitializeParams(id);
            this.model.NomEcran = NomsInternesEcran.InformationsSpecifiquesRisque;
            LoadInfoPage(formattedId);
            this.model.IsISLoaded = IsISLoaded;
            return AutoReadOnlyView("Index",  null, this.model, Model.IsReadOnly && !Model.IsModifHorsAvenant, Model.IsModifHorsAvenant);
        }

        [HttpPost]
        [HandleJsonError]
        [AlbAjaxRedirect]
        public string EnregistrerInfosComplementaires(ModeleInformationsSpecifiquesRisquesPage modelIS) {
            model.Code = modelIS.Code;
            var folder = string.Format("{0}_{1}_{2}", modelIS.Offre.CodeOffre, modelIS.Offre.Version, modelIS.Offre.Type);
            var isReadOnly = GetIsReadOnly(modelIS.TabGuid, folder, modelIS.NumAvenantPage);
            var isModifHorsAvn = GetIsModifHorsAvn(modelIS.TabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(modelIS.NumAvenantPage) ? "0" : modelIS.NumAvenantPage));
            return EnregistrerInfosComplementaires(modelIS, isReadOnly, isModifHorsAvn);
        }

        [ErrorHandler]
        public string EnregistrerInformationsSpecifiques(string branche, string section, string additionalParams, string dataToSave, string splitChars, string strParameters, string cible, string codeOffre, string version, string type, string codeAvn, string codeRsq, string tabGuid, string modeNavig)
        {
            if (modeNavig.ParseCode<ModeConsultation>() != ModeConsultation.Historique && AllowUpdate)
            {
                var questMedical = string.Empty;
                using (var channelClient = ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    questMedical = channelClient.Channel.GetQuestionMedical(codeOffre, version, type, codeRsq, string.Empty, questMedical, false, GetUser());

                    if (!DbIOParam.SaveISToDB(branche, section, cible, additionalParams, dataToSave, splitChars, strParameters)) {
                        throw new AlbFoncException("Erreur de sauvegarde des informations spécifiques");
                    }

                    // Ecriture des traces dans KPCTRLA si valeur questionnaire médical différente
                    questMedical = channelClient.Channel.GetQuestionMedical(codeOffre, version, type, codeRsq, string.Empty, questMedical, true, GetUser());
                }
            }
            return string.Empty;
        }

        [ErrorHandler]
        public ActionResult OpenRechercheOpposition(string codeOrganisme, string nomOrganisme, string context, string typeOppBenef)
        {
            ModeleRechercheOpp toReturn = new ModeleRechercheOpp { Code = codeOrganisme, Nom = nomOrganisme };
            toReturn.lstOppositions = new List<ModeleOrganisme>();
            string value = string.Empty;
            string mode = string.Empty;
            toReturn.context = context;
            if ((!string.IsNullOrEmpty(codeOrganisme) || !string.IsNullOrEmpty(nomOrganisme) || !string.IsNullOrEmpty(context)))
            {
                if (!string.IsNullOrEmpty(codeOrganisme))
                {
                    value = codeOrganisme;
                    mode = "Code";
                }

                if (!string.IsNullOrEmpty(nomOrganisme))
                {
                    value = nomOrganisme;
                    mode = "Name";
                }

                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var contexte = channelClient.Channel;
                    toReturn.lstOppositions = new List<ModeleOrganisme>();
                    var result = contexte.OrganismesGet(value, mode, typeOppBenef);
                    if (result.Any())
                        result.ForEach(organisme => toReturn.lstOppositions.Add((ModeleOrganisme)organisme));
                }
            }

            return PartialView("RechercheOppositions", toReturn);
        }

        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string codeOffre, string version, string type, string codeRisque, string tabGuid, string modeNavig) {
            return RedirectToAction(job, cible, new { id = AlbParameters.BuildFullId(
                new Folder(new[] { codeOffre, version, type }),
                new[] { codeRisque },
                tabGuid,
                null,
                modeNavig)
            });
        }

        /// <summary>
        /// Ouvre la popup contenant les informations détaillées de l'opposition en paramètre
        /// </summary>
        [ErrorHandler]
        public ActionResult ObtenirOppositionDetails(string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string codeAvn, string tabGuid, string codeRisque, string idOpposition, string mode, string appliqueA, string modeNavig, string typeDest)
        {
            model.Code = codeRisque;
            ModeleOpposition toReturn = null;
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var contexte = channelClient.Channel;
                var result = contexte.ObtenirDetailOpposition(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeAvn, codeRisque, idOpposition, mode, modeNavig.ParseCode<ModeConsultation>(), typeDest);
                if (result != null)
                {
                    var listType = result.TypesFinancement;
                    var listObjet = result.ObjetsRisque;
                    toReturn = (ModeleOpposition)result;

                    toReturn.TypesFinancement = listType.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                    if (listObjet != null)
                        toReturn.ObjetsRisque = listObjet.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Selected = true }).ToList();
                    if (!string.IsNullOrEmpty(toReturn.TypeFinancement))
                        toReturn.TypesFinancement.Find(t => t.Value == toReturn.TypeFinancement).Selected = true;
                    toReturn.ListTypesDest = LstTypesDest;
                    toReturn.TypeDest = string.IsNullOrEmpty(toReturn.TypeDest) ? "IN" : toReturn.TypeDest;
                }

            }

            //Définition du mode d'affichage des détails (création, édition, suppression)
            if (toReturn == null)
                toReturn = new ModeleOpposition();

            //Gestion du mode readonly
            var folder = string.Format("{0}_{1}_{2}", codeOffreContrat, versionOffreContrat, typeOffreContrat);
            model.IsReadOnly = GetIsReadOnly(tabGuid, folder, codeAvn);
            model.IsModifHorsAvenant = GetIsModifHorsAvn(tabGuid, string.Format("{0}_{1}", folder, string.IsNullOrEmpty(codeAvn) ? "0" : codeAvn));
            toReturn.Mode = model.IsReadOnly ? "R" : mode;

            #region Récupération des objets liés au risque
            if (typeOffreContrat == "O")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient = client.Channel;
                    model.Offre = new Offre_MetaModel();
                    if (!string.IsNullOrEmpty(versionOffreContrat))
                    {
                        int version = 0;
                        if (int.TryParse(versionOffreContrat, out version)) {
                            model.Offre.LoadOffre(policeServicesClient.OffreGetDto(codeOffreContrat, version, typeOffreContrat, model.ModeNavig.ParseCode<ModeConsultation>()));
                        }
                    }
                }
            }
            else if (typeOffreContrat == "P")
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = channelClient.Channel;
                    model.Contrat = serviceContext.GetContrat(codeOffreContrat, versionOffreContrat, typeOffreContrat, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
            }

            if (model.Offre != null)
            {
                RisqueDto currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(codeRisque));
                if (toReturn.ObjetsRisque != null)
                    toReturn.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = toReturn.ObjetsRisque.Exists(n => n.Value == m.Code.ToString()), Title = m.Descriptif }).ToList();
                else
                    toReturn.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = true, Title = m.Descriptif }).ToList();
                toReturn.LibelleRisque = currentRsq.Descriptif;
            }
            else if (model.Contrat != null)
            {
                RisqueDto currentRsq = model.Contrat.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(codeRisque));
                if (toReturn.ObjetsRisque != null)
                    toReturn.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = toReturn.ObjetsRisque.Exists(n => n.Value == m.Code.ToString()), Title = m.Descriptif }).ToList();
                else
                    toReturn.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = true, Title = m.Descriptif }).ToList();
                toReturn.LibelleRisque = currentRsq.Descriptif;
            }

            #endregion
            if (appliqueA == "A") {
                return PartialView("DetailOpposition", toReturn);
            }
            return PartialView("EditOpposition", toReturn);
        }

        /// <summary>
        /// Enregistre les modifications apportées à l'opposition en paramètre (création, modification, suppression)
        /// </summary>
        /// <param name="codeOffreContrat"></param>
        /// <param name="versionOffreContrat"></param>
        /// <param name="typeOffreContrat"></param>
        /// <param name="codeRisque"></param>
        /// <param name="data"></param>
        /// <param name="objets"></param>
        /// <returns></returns>
        [ErrorHandler]
        public ActionResult EnregistrerModificationOpposition(string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string codeRisque, string data, string objets, string modeNavig)
        {
            var serialiserOpposition = AlbJsExtendConverter<OppositionDto>.GetSerializer();
            var oppositionDto = serialiserOpposition.ConvertToType<List<OppositionDto>>(serialiserOpposition.DeserializeObject(data)).FirstOrDefault();

            if (!string.IsNullOrEmpty(oppositionDto.Description))
                oppositionDto.Description = oppositionDto.Description.Replace("'", "''");

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var contexte = channelClient.Channel;
                contexte.MiseAJourOpposition(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeRisque, oppositionDto, objets, GetUser());
            }

            //Récupération de l'ensemble des oppositions
            ModeleInformationsSpecifiquesRisquesPage model = new ModeleInformationsSpecifiquesRisquesPage
            {
                ListOppositions = ObtenirListeOppositions(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeRisque, modeNavig)
            };
            return PartialView("ListeOppositions", model);
        }

        /// <summary>
        /// Renvoi la liste des oppositions de l'offre/contrat et risque en parametre
        /// </summary>
        /// <param name="codeOffreContrat"></param>
        /// <param name="versionOffreContrat"></param>
        /// <param name="typeOffreContrat"></param>
        /// <param name="codeRisque"></param>
        /// <returns></returns>
        [ErrorHandler]
        public List<ModeleOpposition> ObtenirListeOppositions(string codeOffreContrat, string versionOffreContrat, string typeOffreContrat, string codeRisque, string modeNavig)
        {
            #region Récupération de l'offre/contrat
            if (typeOffreContrat == "O")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient = client.Channel;
                    model.Offre = new Offre_MetaModel();
                    if (!string.IsNullOrEmpty(versionOffreContrat))
                    {
                        int version = 0;
                        if (int.TryParse(versionOffreContrat, out version))
                            model.Offre.LoadOffre(policeServicesClient.OffreGetDto(codeOffreContrat, version, typeOffreContrat, model.ModeNavig.ParseCode<ModeConsultation>()));
                        //model.Offre.LoadOffre(policeServicesClient.OffreGetDto(codeOffreContrat, version, typeOffreContrat, model.ModeNavig));
                    }
                }
            }
            else if (typeOffreContrat == "P")
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = channelClient.Channel;
                    model.Contrat = serviceContext.GetContrat(codeOffreContrat, versionOffreContrat, typeOffreContrat, model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                }
            }
            #endregion

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var contexte = channelClient.Channel;
                var listeDto = contexte.ObtenirListeOppositions(codeOffreContrat, versionOffreContrat, typeOffreContrat, codeRisque, modeNavig.ParseCode<ModeConsultation>());
                var toReturn = listeDto.Select(item => (ModeleOpposition)item).ToList();

                #region gestion du label "s'applique à"
                if (model.Offre != null)
                {
                    RisqueDto currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(codeRisque));
                    foreach (ModeleOpposition opposition in toReturn)
                    {
                        if (opposition.ObjetsRisque != null)
                            opposition.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = opposition.ObjetsRisque.Exists(n => n.Value == m.Code.ToString()), Title = m.Descriptif }).ToList();
                        else
                            opposition.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = true, Title = m.Descriptif }).ToList();

                        opposition.AppliqueALabel = "s'applique à : ";
                        foreach (AlbSelectListItem item in opposition.ObjetsRisque)
                        {
                            if (item.Selected)
                                opposition.AppliqueALabel += item.Text + " ,";
                        }
                        opposition.AppliqueALabel = opposition.AppliqueALabel.Substring(0, opposition.AppliqueALabel.Length - 1);
                    }
                }
                else if (model.Contrat != null)
                {
                    RisqueDto currentRsq = model.Contrat.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(codeRisque));
                    foreach (ModeleOpposition opposition in toReturn)
                    {
                        if (opposition.ObjetsRisque != null)
                            opposition.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = opposition.ObjetsRisque.Exists(n => n.Value == m.Code.ToString()), Title = m.Descriptif }).ToList();
                        else
                            opposition.ObjetsRisque = currentRsq.Objets.Select(m => new AlbSelectListItem { Value = m.Code.ToString(), Text = m.Descriptif, Selected = true, Title = m.Descriptif }).ToList();

                        opposition.AppliqueALabel = "s'applique à : ";
                        foreach (AlbSelectListItem item in opposition.ObjetsRisque)
                        {
                            if (item.Selected)
                                opposition.AppliqueALabel += item.Text + " ,";
                        }
                        opposition.AppliqueALabel = opposition.AppliqueALabel.Substring(0, opposition.AppliqueALabel.Length - 1);
                    }
                }
                #endregion

                return toReturn;
            }
        }

        protected override bool GetInfoSpeReadonly(Folder folder) {
            int rsq = int.TryParse(this.model.Code, out int r) ? r : default;
            if (rsq == default) {
                return true;
            }
            using (var client = ServiceClientFactory.GetClient<IRisquePort>()) {
                return client.Channel.IsAvnDisabled(folder.Adapt<AffaireId>(), rsq);
            }
        }

        #region Méthodes Privées

        protected override void LoadInfoPage(string id)
        {
            string[] tId = id.Split('_');

            model.Bandeau = null;
            if (tId[2] == "O")
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var policeServicesClient = client.Channel;
                    model.Offre = new Offre_MetaModel();
                    model.Offre.LoadOffre(policeServicesClient.OffreGetDto(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>()));
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                }
            }
            else if (tId[2] == "P")
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var serviceContext = channelClient.Channel;
                    model.Contrat = serviceContext.GetContrat(tId[0], tId[1], tId[2], model.NumAvenantPage, model.ModeNavig.ParseCode<ModeConsultation>());
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            break;
                        default:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                }
            }
            model.PageTitle = "Informations Spécifiques Risque";
            if (model.Offre != null || model.Contrat != null)
            {
                model.AfficherBandeau = base.DisplayBandeau(true, id);
                model.AfficherNavigation = model.AfficherBandeau;
            }
            string branche = string.Empty;
            string cible = string.Empty;
            if (!string.IsNullOrEmpty(tId[3].ToString()))
            {
                model.Code = tId[3].ToString();

                if (model.Offre != null)
                {
                    RisqueDto currentRsq = model.Offre.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));
                    model.RisqueIndexe = currentRsq.RisqueIndexe;
                    model.Assiette = currentRsq.Assiette;
                    model.LCI = currentRsq.LCI;
                    model.Franchise = currentRsq.Franchise;
                    model.RegimeTaxe = currentRsq.RegimeTaxe;
                    model.CATNAT = currentRsq.CATNAT;
                    model.DescriptifRisque = currentRsq.Descriptif;

                    if (model.Offre.Branche.Code != _branche || currentRsq.Cible.Code != _cible)
                    {
                        _branche = model.Offre.Branche.Code;
                        _cible = currentRsq.Cible.Code;
                        _lstTypesRegularisation = null;
                    }

                    model.ListOppositions = ObtenirListeOppositions(tId[0], tId[1], tId[2], tId[3], model.ModeNavig);

                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Offre.CodeOffre + "_" + model.Offre.Version + "_" + model.Offre.Type, model.NumAvenantPage);
                    branche = model.Offre.Branche.Code;
                    cible = currentRsq.Objets[0].Cible.Code;
                    model.Valeur = model.Offre.Valeur.ToString();
                    model.IndiceRef = model.Offre.IndiceReference.Code;
                    model.Parameters = string.Format("&branche={0}&section={1}&",
                                                       model.Offre.Branche.Code,
                                                       AlbConstantesMetiers.INFORMATIONS_RISQUES);
                    model.TauxAppel = currentRsq.TauxAppel;
                    model.TypeRegularisation = currentRsq.TypeRegularisation;
                    model.IsRegularisable = currentRsq.IsRegularisable == "O";

                    model.PartBenef = currentRsq.PartBenef;
                    model.PartBenefGenerale = model.Offre.PartBenef;
                    model.NbYear = currentRsq.NbYear;
                    model.Ristourne = currentRsq.Ristourne;
                    model.CotisRetenue = currentRsq.CotisRetenue;
                    model.Seuil = currentRsq.Seuil;
                    model.TauxComp = currentRsq.TauxComp;
                    model.TauxMaxi = currentRsq.TauxMaxi;
                    model.PrimeMaxi = currentRsq.PrimeMaxi;
                }
                else if (model.Contrat != null)
                {
                    RisqueDto currentRsq = model.Contrat.Risques.FirstOrDefault(x => x.Code == Convert.ToInt32(tId[3].ToString()));
                    model.RisqueIndexe = currentRsq.RisqueIndexe;
                    model.Assiette = currentRsq.Assiette;
                    model.LCI = currentRsq.LCI;
                    model.Franchise = currentRsq.Franchise;
                    model.RegimeTaxe = currentRsq.RegimeTaxe;
                    model.CATNAT = currentRsq.CATNAT;
                    model.DescriptifRisque = currentRsq.Descriptif;

                    if (model.Contrat.Branche != _branche || currentRsq.Cible.Code != _cible)
                    {
                        _branche = model.Contrat.Branche;
                        _cible = currentRsq.Cible.Code;
                        _lstTypesRegularisation = null;
                    }

                    model.ListOppositions = ObtenirListeOppositions(tId[0], tId[1], tId[2], tId[3], model.ModeNavig);

                    model.IsReadOnly = GetIsReadOnly(model.TabGuid, model.Contrat.CodeContrat + "_" + model.Contrat.VersionContrat + "_" + model.Contrat.Type, model.NumAvenantPage);
                    this.model.IsModifHorsAvenant = IsModifHorsAvenant;
                    branche = model.Contrat.Branche;
                    cible = currentRsq.Objets[0].Cible.Code;
                    model.Valeur = model.Contrat.Valeur.ToString();
                    model.IndiceRef = model.Contrat.IndiceReference;
                    model.Parameters = string.Format("&branche={0}&section={1}&", model.Contrat.Branche, AlbConstantesMetiers.INFORMATIONS_RISQUES);
                    model.TauxAppel = currentRsq.TauxAppel;
                    model.TypeRegularisation = currentRsq.TypeRegularisation;
                    model.IsRegularisable = currentRsq.IsRegularisable == "O";

                    model.PartBenef = currentRsq.PartBenef;
                    model.PartBenefGenerale = model.Contrat.PartBenefDB;
                    model.NbYear = currentRsq.NbYear;
                    model.Ristourne = currentRsq.Ristourne;
                    model.CotisRetenue = currentRsq.CotisRetenue;
                    model.Seuil = currentRsq.Seuil;
                    model.TauxComp = currentRsq.TauxComp;
                    model.TauxMaxi = currentRsq.TauxMaxi;
                    model.PrimeMaxi = currentRsq.PrimeMaxi;
                }
                model.ListTypesRegularisation = LstTypesRegularisation;
                model.ListPbBNS = LstPBBNS(model.PartBenefGenerale);
                var elemPar = id.Split('_');
                var sbParams = new StringBuilder();
                if (model.Offre != null)
                    sbParams.Append(model.Offre.Type + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
                else if (model.Contrat != null)
                    sbParams.Append(model.Contrat.Type + MvcApplication.SPLIT_CONST_HTML + elemPar[0].PadLeft(9) + MvcApplication.SPLIT_CONST_HTML);
                sbParams.Append(elemPar[1] + MvcApplication.SPLIT_CONST_HTML);
                sbParams.Append(elemPar[3]);
                model.SpecificParameters = sbParams.ToString();
            }
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServicesClient = client.Channel;
                List<ParametreDto> regimeTaxes = policeServicesClient.RegimeTaxeGet(branche, cible).ToList();
                model.RegimesTaxe = regimeTaxes.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                var indicesRefLst = policeServicesClient.IndicesReferenceGet();
                model.IndiceRefLibelle = indicesRefLst.Find(elm => elm.Code == model.IndiceRef) != null ? indicesRefLst.Find(elm => elm.Code == model.IndiceRef).Libelle : string.Empty;

                if (this.model.IsModifHorsAvenant)
                {
                    model.IsHorsAvnRegularisable = policeServicesClient.GetModifHorsAvnIsRegularisable(
                        model.Contrat.CodeContrat,
                        (int)model.Contrat.VersionContrat, model.Contrat.Type,
                        model.Contrat.NumAvenant, model.Code);
                }
            }


            #region Navigation Arbre
            SetArbreNavigation();
            #endregion

            #region Bandeau
            model.Bandeau = null;
            SetBandeauNavigation(id);
            #endregion

            #region LCIFranchise
            LoadLCIFranchise(tId[0], tId[1], tId[2], tId[3], model.NumAvenantPage, model.ModeNavig);
            #endregion



            #region Trace Regularisable
             CanDoRegularisable(model);
            #endregion

        }

        private static RisqueDto buildModifierOffre(ModeleInformationsSpecifiquesRisquesPage model)
        {
            var toReturn = new RisqueDto
            {
                AdresseRisque = new AdressePlatDto(),
                Code = Convert.ToInt32(model.Code),
                RisqueIndexe = model.RisqueIndexe,
                Assiette = model.Assiette,
                LCI = model.LCI,
                Franchise = model.Franchise,
                RegimeTaxe = model.RegimeTaxe,
                CATNAT = model.CATNAT,
                TauxAppel = model.TauxAppel,
                IsRegularisable = model.IsRegularisable ? "O" : "N",
                TypeRegularisation = model.TypeRegularisation,
                PartBenef = model.PartBenef,
                NbYear = model.NbYear,
                Ristourne = model.Ristourne,
                CotisRetenue = model.CotisRetenue,
                Seuil = model.Seuil,
                TauxComp = model.TauxComp,
                TauxMaxi = model.TauxMaxi,
                PrimeMaxi = model.PrimeMaxi
            };
            return toReturn;
        }

        private void SetBandeauNavigation(string id)
        {

            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                //Gestion des Etapes
                if (model.Offre != null)
                {
                    model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Offre.CodeOffre,
                        Version = model.Offre.Version
                    };
                }
                else if (model.Contrat != null)
                {
                    string typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    switch (typeAvt)
                    {
                        case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
                            model.Bandeau.StyleBandeau = model.ScreenType;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_REGULMODIF;
                            break;
                        case AlbConstantesMetiers.TYPE_AVENANT_REMISE_EN_VIGUEUR:
                            model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REMISEVIGUEUR;
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                            break;
                        default:
                            model.Bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                    }
                    model.Navigation = new Navigation_MetaModel
                    {
                        Etape = Navigation_MetaModel.ECRAN_RISQUEETGARANTIE,
                        IdOffre = model.Contrat.CodeContrat,
                        Version = int.Parse(model.Contrat.VersionContrat.ToString())
                    };
                }
            }
        }
        private void SetArbreNavigation()
        {
            if (model.Offre != null)
            {
                //Affichage de la navigation latérale en arboresence
                model.NavigationArbre = GetNavigationArbre("Risque", codeRisque: Convert.ToInt32(model.Code));
            }
            else if (model.Contrat != null)
            {
                model.NavigationArbre = GetNavigationArbreAffaireNouvelle("Risque", codeRisque: Convert.ToInt32(model.Code));
            }
        }
        private void LoadLCIFranchise(string codeOffre, string version, string type, string codeRisque, string codeAvn, string modeNavig)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
            {
                var commonOffreClient = client.Channel;
                var lciFranchiseUnitesTypes = commonOffreClient.GetLCIFranchiseUnitesTypes(codeOffre, version, type, codeAvn, modeNavig.ParseCode<ModeConsultation>());
                List<AlbSelectListItem> unitesLCI = lciFranchiseUnitesTypes.UnitesLCI.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                List<AlbSelectListItem> typesLCI = lciFranchiseUnitesTypes.TypesLCI.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                List<AlbSelectListItem> unitesFranchise = lciFranchiseUnitesTypes.UnitesFranchise.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                List<AlbSelectListItem> typesFranchise = lciFranchiseUnitesTypes.TypesFranchise.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList();
                LCIFranchiseDto lciDto = new LCIFranchiseDto();
                LCIFranchiseDto franchiseDto = new LCIFranchiseDto();

                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = channelClient.Channel;
                    lciDto = serviceContext.GetLCIFranchise(codeOffre, version, type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.LCI, AlbConstantesMetiers.TypeAppel.Risque, modeNavig.ParseCode<ModeConsultation>());
                    franchiseDto = serviceContext.GetLCIFranchise(codeOffre, version, type, codeAvn, codeRisque, AlbConstantesMetiers.ExpressionComplexe.Franchise, AlbConstantesMetiers.TypeAppel.Risque, modeNavig.ParseCode<ModeConsultation>());
                }

                if (!string.IsNullOrEmpty(lciDto.Unite))
                {
                    var sItem = unitesLCI.FirstOrDefault(x => x.Value == lciDto.Unite);
                    if (sItem != null)
                        sItem.Selected = true;
                }

                if (!string.IsNullOrEmpty(lciDto.Type))
                {
                    var sItem = typesLCI.FirstOrDefault(x => x.Value == lciDto.Type);
                    if (sItem != null)
                        sItem.Selected = true;
                }
                model.LCIRisque = new Models.ModelesLCIFranchise.ModeleLCIFranchise
                {
                    TypeVue = AlbConstantesMetiers.ExpressionComplexe.LCI,
                    TypeAppel = AlbConstantesMetiers.TypeAppel.Risque,
                    Valeur = lciDto.Valeur.ToString(),
                    Unite = lciDto.Unite,
                    Unites = unitesLCI,
                    Type = lciDto.Type,
                    Types = typesLCI,
                    LienComplexe = lciDto.LienComplexe.ToString(),
                    LibComplexe = lciDto.LibComplexe,
                    CodeComplexe = lciDto.CodeComplexe

                };


                if (!string.IsNullOrEmpty(franchiseDto.Unite))
                {
                    var sItem = unitesFranchise.FirstOrDefault(x => x.Value == franchiseDto.Unite);
                    if (sItem != null)
                        sItem.Selected = true;
                }

                if (!string.IsNullOrEmpty(franchiseDto.Type))
                {
                    var sItem = typesFranchise.FirstOrDefault(x => x.Value == franchiseDto.Type);
                    if (sItem != null)
                        sItem.Selected = true;
                }
                model.FranchiseRisque = new Models.ModelesLCIFranchise.ModeleLCIFranchise
                {
                    TypeVue = AlbConstantesMetiers.ExpressionComplexe.Franchise,
                    TypeAppel = AlbConstantesMetiers.TypeAppel.Risque,
                    Valeur = franchiseDto.Valeur.ToString(),
                    Unite = franchiseDto.Unite,
                    Unites = unitesFranchise,
                    Type = franchiseDto.Type,
                    Types = typesFranchise,
                    LienComplexe = franchiseDto.LienComplexe.ToString(),
                    LibComplexe = franchiseDto.LibComplexe,
                    CodeComplexe = franchiseDto.CodeComplexe
                };
            }
        }

        /// <summary>
        /// B3203
        /// Régle du modif hors avenant au niveau du popup information complementaire risque
        /// </summary>
        /// <param name="model"></param>
        /// <param name="contratId"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="numAvn"></param>
        private void CanDoRegularisable(ModeleInformationsSpecifiquesRisquesPage model)
        {
            var hasTraceModifHorsAvn = false;
            // 00 : par defaut CanDoRegularisable = IsRegularisation
            model.CanDoRegularisable = model.IsRegularisable;
            // 01 : cas modif hors avenant et un contrat qui existe
            if (model.IsModifHorsAvenant && model.Contrat != null) {
                // 02 : Vérification du trace régul.
                using (var channelClient = ServiceClientFactory.GetClient<IRisquesGaranties>()) {
                    hasTraceModifHorsAvn = channelClient.Channel.HaveTraceRegularisation(
                        model.Contrat.CodeContrat,
                        model.Code,
                        model.Contrat.VersionContrat.ToString(),
                        model.Contrat.Type,
                        model.Contrat.NumAvenant.ToString());
                }
                if (hasTraceModifHorsAvn) {
                    model.CanDoRegularisable = false;
                }
            }
        }
        /// <summary>
        /// B3203
        /// Validation du popup informations complémentaires risque du l'écran Risque
        /// </summary>
        /// <param name="currentInfoSepecificRisque"></param>
        /// <param name="modeNavig"></param>
        /// <param name="isModifHorsAvn"></param>
        private static string EnregistrerInfosComplementaires(ModeleInformationsSpecifiquesRisquesPage modelIS, bool isReadOnly, bool isModifHorsAvn)
        {
            if (modelIS != null) {
                modelIS.InitPoliceId();
                using (var client = ServiceClientFactory.GetClient<IRisquesGaranties>())
                {
                    var serviceContext = client.Channel;
                    if (isModifHorsAvn)
                    {
                        var currentIsRegularisable = modelIS.IsRegularisable;
                        // Récuperation du valeur régularisation du risque de la BDD
                        var savedIsRegularisable = serviceContext.GetIsRegularisation(modelIS.CodePolicePage, modelIS.Code.ToString(), modelIS.VersionPolicePage);
                        // Enregistrmeent des informations complémentaires risque
                        EnregisterSpecifiquesRisque(modelIS, isReadOnly, isModifHorsAvn);
                        if (savedIsRegularisable != null)
                        {
                            // Vérification si case du risque régularisable a été décoché ou coché
                            if (currentIsRegularisable != savedIsRegularisable)
                            {
                                using (var clientTrace = ServiceClientFactory.GetClient<IAffaireNouvelle>())
                                {
                                    // Trace contrat dans KPHAVH avec la nouvelle valeur du régularisation
                                    clientTrace.Channel.TraceContratInfoModifHorsAvn(modelIS.CodePolicePage, modelIS.VersionPolicePage, modelIS.TypePolicePage, modelIS.NumAvenantPage.ToString(), GetUser() ,modelIS.Code, true);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Enregistrmeent des informations complémentaires risque
                        EnregisterSpecifiquesRisque(modelIS, isReadOnly, isModifHorsAvn);
                    }
                }
            }

            return string.Empty;

        }
        /// <summary>
        /// Enregistrmeent des informations complémentaires risque
        /// </summary>
        /// <param name="modelInfos"></param>
        /// <param name="isReadOnly"></param>
        /// <param name="isModifHorsAvn"></param>
        private static void EnregisterSpecifiquesRisque(ModeleInformationsSpecifiquesRisquesPage modelInfos, bool isReadOnly, bool isModifHorsAvn)
        {
            if ((!isReadOnly || isModifHorsAvn) && ModeConsultation.Historique != modelInfos.ModeNavig.ParseCode<ModeConsultation>())
            {
                string msgError = string.Empty;
                var offre = new OffreDto {
                    CodeOffre = modelInfos.Offre.CodeOffre,
                    Version = modelInfos.Offre.Version,
                    Type = modelInfos.Offre.Type,
                    NumAvenant = int.TryParse(modelInfos.NumAvenantPage, out int i) && i > 0 ? i : default
                };
                var risques = new List<RisqueDto> { buildModifierOffre(modelInfos) };
                offre.Risques = risques;

                using (var serviceClient = ServiceClientFactory.GetClient<IFormule>()) {
                    var lciRisque = modelInfos.LCIRisque is null ? null : new Albingia.Kheops.DTO.ValeursUniteDto {
                        CodeBase = modelInfos.LCIRisque.Type,
                        CodeUnite = modelInfos.LCIRisque.Unite,
                        ValeurActualise = decimal.TryParse(modelInfos.LCIRisque.Valeur?.Replace(".", ","), out var d) ? d : default,
                        IdCPX = long.TryParse(modelInfos.LCIRisque.LienComplexe, out long l) && l > 0 ? l : default(long?)
                    };
                    var franchiseRisque = modelInfos.FranchiseRisque is null ? null : new Albingia.Kheops.DTO.ValeursUniteDto {
                        CodeBase = modelInfos.FranchiseRisque.Type,
                        CodeUnite = modelInfos.FranchiseRisque.Unite,
                        ValeurActualise = decimal.TryParse(modelInfos.FranchiseRisque.Valeur?.Replace(".", ","), out d) ? d : default,
                        IdCPX = long.TryParse(modelInfos.FranchiseRisque.LienComplexe, out l) && l > 0 ? l : default(long?)
                    };
                    if (lciRisque != null)
                    {
                        lciRisque.ValeurOrigine = lciRisque.ValeurActualise;
                    }
                    if (franchiseRisque != null)
                    {
                        franchiseRisque.ValeurOrigine = franchiseRisque.ValeurActualise;
                    }
                    serviceClient.Channel.SaveInfosComplementairesRisque(
                        offre,
                        lciRisque,
                        franchiseRisque,
                        isModifHorsAvn,
                        modelInfos.ForceAllowCatnat ? new[] { $"{nameof(RisqueDto.CATNAT)},{nameof(RisqueDto.RegimeTaxe)}" } : null);
                }
            }
        }

        #endregion
    }
}
