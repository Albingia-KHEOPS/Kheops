using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache;
using ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;

using EmitMapper;
using Mapster;
using OP.WSAS400.DTO.Adresses;
using OP.WSAS400.DTO.Ecran.CreationSaisie;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Risque;
using OP.WSAS400.DTO.Offres.Risque.Objet;
using OP.WSAS400.DTO.Offres.Assures;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Partenaire;
using OP.WSAS400.DTO.Personnes;
using OP.WSAS400.DTO.Ecran.DetailsRisque;
using OPServiceContract.IAdministration;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ISaisieCreationOffre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class CreationSaisieController : ControllersBase<ModeleCreationSaisiePage>
    {
        public override bool IsReadonly => base.IsReadonly && this.model.EditMode;

        [ErrorHandler]
        [AlbVerifLockedOffer("id")]
        public ActionResult Index(string id=null)
        {
             if (!string.IsNullOrEmpty(id)) {
              id = InitializeParams(id);
             }
              
            LoadInfoPage(id);
            model.PageTitle = "Création saisie";
            model.AfficherNavigation = true;
            model.Navigation = new Navigation_MetaModel
            {
                Etape = Navigation_MetaModel.ECRAN_SAISIE
            };

           

            if (model.EditMode) {
                model.NavigationArbre = GetNavigationArbre("InfoSaisie");
            }
            else {
                model.NavigationArbre = GetNavigationArbre("InfoSaisie", returnEmptyTree: true);
            }
            model.NavigationArbre.IsReadOnly = model.IsReadOnly;
            model.PageTitle = "Création saisie";

            model.Bandeau = null;
            model.AfficherBandeau = DisplayBandeau(true, id);
            model.AfficherNavigation = model.AfficherBandeau;
            model.AfficherArbre = true;
            if (model.AfficherBandeau)
            {
                model.Bandeau = GetInfoBandeau(id.Split('_').Skip(2).FirstOrDefault());
                model.Navigation.IdOffre = model.Offre.CodeOffre;
                model.Navigation.Version = model.Offre.Version;
                model.Bandeau.HasDoubleSaisie = false;//on simule l'absence de double saisie dans cet écran pour ne pas afficher le bouton d'accès
            }

            if (model.InformationSaisie.InformationTemplate != null && !model.CopyMode)
            {
                model.CabinetCourtage.IsReadOnlyDisplay = true;
                model.PreneurAssurance.IsReadOnlyDisplay = true;
                model.PreneurAssurance.PreneurEstAssure = true;
            }

            model.PageEnCours = NomsInternesEcran.CreationSaisie.ToString();
            // Permet aux vues partielles d'identifier si la saisie en cours est un Canvas            
            SetViewBagCanevas();

            return View(model);
        }

        [ErrorHandler]
        [AlbAjaxRedirect]
        public RedirectToRouteResult Redirection(string cible, string job, string param, string paramRedirect)
        {
            //Redirection vers la page sélectionnée dans le menu
            if (!string.IsNullOrEmpty(paramRedirect))
            {
                var tabParamRedirect = paramRedirect.Split('/');
                return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
            }
            return RedirectToAction(job, cible, new { id = param });
        }
        [AlbAjaxRedirect]
        public void OffreRisquesSupprimer(string id, string tabGuid, string modeNavig, string addParamType, string addParamValue)
        {

            string codeOffre = id.Split('_')[0];
            int? version = Convert.ToInt32(id.Split('_')[1]);
            string type = id.Split('_')[2];
            //int codeRisque = Convert.ToInt32(id.Split('_')[3]);
            string codeBranche = id.Split('_')[3];
            List<RisqueDto> risques = new List<RisqueDto>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var policeServicesClient = channelClient.Channel;
                risques = policeServicesClient.ObtenirIDRisques(codeOffre, version);
            }
            
                
            
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var screenClient = client.Channel;
                DetailsRisqueDelQueryDto query = new DetailsRisqueDelQueryDto();
                query.offre = new OffreDto { CodeOffre = codeOffre, Version = version, Branche = new BrancheDto { Code = codeBranche, Cible = new CibleDto() }, Risques = risques, Type = type };
                screenClient.DetailsRisqueDel(query);
                // return RedirectToAction("Index", "MatriceRisque", new { id = codeOffre + "_" + version.ToString() + "_" + type + tabGuid + BuildAddParamString(addParamType, addParamValue) + GetFormatModeNavig(modeNavig) });
            }

        }
        [ErrorHandler]
        public ActionResult GetCibles(string codeBranche)
        {
            SetViewBagCanevas();
            ModeleInfoSaisieCible model = new ModeleInfoSaisieCible
            {
                Cible = string.Empty,
                Cibles = GetListeCibles(codeBranche)
            };
            return PartialView("Cible_InformationSaisie", model);
        }

        [ErrorHandler]
        public ActionResult GetTemplates(string codeBranche, string codeCible)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext = channelClient.Channel;
                var result = serviceContext.GetDefaultTemplateOffre(AlbConstantesMetiers.TYPE_OFFRE, codeCible, codeBranche, model.ModeNavig.ParseCode<ModeConsultation>());
                if (result != null)
                {
                    var templateMetaModel = new Offre_MetaModel();
                    templateMetaModel.LoadOffre(ObjectMapperManager.DefaultInstance.GetMapper<OffreDto, OffreDto>().Map(result));
                    return LoadDataTemplate(templateMetaModel, codeBranche, codeCible, result.CodeOffre);
                }
            }
            return null;
        }

        [ErrorHandler]
        public ActionResult GetTemplatePage(string codeBranche, string codeCible, string codeTemplate)
        {
            if (!string.IsNullOrEmpty(codeTemplate))
            {
                return LoadDataTemplate(CacheModels.GetOffreFromCache(codeTemplate, 0, AlbConstantesMetiers.TYPE_OFFRE), codeBranche, codeCible, codeTemplate);
            }
            return LoadDataTemplate(null, codeBranche, codeCible, string.Empty);
        }

        [ErrorHandler]
        public ActionResult GetListeMotsCles(string codeBranche, string codeCible)
        {
            var toReturn = new ModeleDescription();
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

        [AlbAjaxRedirect]
        [HandleJsonError]
        public RedirectToRouteResult CreationSaisieEnregistrement(ModeleCreationSaisiePage createModel)
        {
            if (this.model.Type != AlbConstantesMetiers.TYPE_OFFRE) {
                throw new AlbFoncException(string.Format("Erreur d'enregistrement de l'offre n°", this.model.CodeOffre.Trim()), true, true);
            }

            if (!this.model.ModeNavig.Contains("modeNavig"))
            {
                this.model.ModeNavig = GetFormatModeNavig(this.model.ModeNavig);
            }

            if (this.model.Version == null)
            {
                this.model.Version = 0;
                this.model.Offre.Version = 0;
            }

            CreationSaisieSetQueryDto query = null;
            CreationSaisieSetResultDto result = null;
            if (!IsReadonly)
            {
                query = new CreationSaisieSetQueryDto { Offre = new OffreDto() };
                query.Offre.CodeOffreCopy = this.model.CodeOffreCopy;
                query.Offre.VersionCopy = this.model.VersionCopy;
                query.Offre.CopyMode = this.model.CopyMode;
                query.Offre.CodeOffre = this.model.CodeOffre;
                query.Offre.Version = this.model.Version;
                query.Offre.Type = this.model.Type;
                query.Offre.CabinetGestionnaire = new CabinetCourtageDto { Code = (this.model.CabinetCourtage.CodeCabinetCourtage.HasValue) ? this.model.CabinetCourtage.CodeCabinetCourtage.Value : 0 };

                query.Offre.CabinetApporteur = new CabinetCourtageDto { Code = query.Offre.CabinetGestionnaire.Code };
                query.Offre.PreneurAssurance = new AssureDto
                {
                    Code = this.model.PreneurAssurance.CodePreneurAssurance.ToString(),
                    PreneurEstAssure = this.model.PreneurAssurance.PreneurEstAssure
                };
                query.Offre.Souscripteur = new SouscripteurDto { Code = this.model.InformationSaisie.CodeSouscripteur };
                query.Offre.Gestionnaire = new GestionnaireDto { Id = this.model.InformationSaisie.CodeGestionnaire };
                query.Offre.Branche = new BrancheDto { Code = this.model.InformationSaisie.Branche, Cible = new CibleDto { Code = this.model.InformationSaisie.InformationCible.Cible } };
                query.Offre.Cible = new CibleDto { Code = this.model.InformationSaisie.InformationCible.Cible };
                query.Offre.RefChezCourtier = this.model.CabinetCourtage.Reference;
                query.Offre.CodeInterlocuteur = this.model.CabinetCourtage.CodeInterlocuteur;
                query.Offre.DateSaisie = this.model.InformationSaisie.DateSaisie;
                
                string cpFormat = !string.IsNullOrEmpty(this.model.ContactAdresse.CodePostal) 
                    ? this.model.ContactAdresse.CodePostal.Length >= 3 
                        ? this.model.ContactAdresse.CodePostal.Substring(this.model.ContactAdresse.CodePostal.Length - 3, 3)
                        : this.model.ContactAdresse.CodePostal
                    : string.Empty;
                string cpCedexFormat = !string.IsNullOrEmpty(this.model.ContactAdresse.CodePostalCedex)
                    ? this.model.ContactAdresse.CodePostalCedex.Length >= 3 
                        ? this.model.ContactAdresse.CodePostalCedex.Substring(this.model.ContactAdresse.CodePostalCedex.Length - 3, 3)
                        : this.model.ContactAdresse.CodePostalCedex
                    : string.Empty;

                query.Offre.AdresseOffre = new AdressePlatDto
                {
                    Batiment = this.model.ContactAdresse.Batiment,
                    NumeroVoie = Int32.TryParse(this.model.ContactAdresse.No.Split(new char[] { '/', '-' })[0], out int numVoie) ? numVoie : -1,
                    NumeroVoie2 = this.model.ContactAdresse.No.Contains("/") || this.model.ContactAdresse.No.Contains("-") ? this.model.ContactAdresse.No.Split(new char[] { '/', '-' })[1] : "",
                    ExtensionVoie = this.model.ContactAdresse.Extension,
                    NomVoie = this.model.ContactAdresse.Voie,
                    BoitePostale = this.model.ContactAdresse.Distribution,
                    CodePostal = int.TryParse(cpFormat, out int cp) ? cp : -1,
                    NomVille = this.model.ContactAdresse.Ville,
                    CodePostalCedex = int.TryParse(cpCedexFormat, out int cpCedex) ? cpCedex : -1,
                    NomCedex = this.model.ContactAdresse.VilleCedex,
                    NomPays = this.model.ContactAdresse.Pays,
                    MatriculeHexavia = this.model.ContactAdresse.MatriculeHexavia,
                    Latitude = this.model.ContactAdresse.Latitude.IsEmptyOrNull() ? 0 : decimal.Parse(this.model.ContactAdresse.Latitude.Replace(".",",")),
                    Longitude = this.model.ContactAdresse.Longitude.IsEmptyOrNull() ? 0 : decimal.Parse(this.model.ContactAdresse.Longitude.Replace(".", ",")),
                    NumeroChrono = this.model.ContactAdresse.NoChrono.HasValue ? this.model.ContactAdresse.NoChrono.Value : 0,
                    Departement = !string.IsNullOrEmpty(this.model.ContactAdresse.CodePostal) && this.model.ContactAdresse.CodePostal.Length == 5 ? this.model.ContactAdresse.CodePostal.Substring(0, 2) : string.Empty
                };

                query.Offre.MotCle1 = this.model.Description.MotClef1;
                query.Offre.MotCle2 = this.model.Description.MotClef2;
                query.Offre.MotCle3 = this.model.Description.MotClef3;
                query.Offre.Descriptif = this.model.Description.Descriptif;
                query.Offre.Observation = this.model.Description.Observation;
                if (!string.IsNullOrEmpty(this.model.Description.Observation))
                {
                    query.Offre.Observation = this.model.Description.Observation.Replace(Environment.NewLine, "<br>");
                }

                ////T 3997 : Vérification des partenaires
               // VerificationPartenairesOffre(this.model);
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICreationOffre>())
                {
                    var serviceContext = channelClient.Channel;
                    result = serviceContext.CreationSaisieEnregistrement(query, GetUser());
                }

                if (!string.IsNullOrEmpty(result.RetourMsg)) {
                    throw new AlbFoncException(result.RetourMsg);
                }
                else if (!query.Offre.CodeOffre.StartsWith("CV") && query.Offre.CabinetGestionnaire.Code == 0) {
                    throw new AlbFoncException(AlbOpConstants.CABINET_GESTIONNAIRE_EMPTY_ERROR);
                }
            }
            return Redirect(query, result);
        }

        #region Méthodes Privées

        protected override void LoadInfoPage(string id)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICreationOffre>())
            {
                var screenClient = channelClient.Channel;
                var query = new CreationSaisieGetQueryDto();
                var result = screenClient.CreationSaisieGet(query);

                if (result != null)
                {
                    model.ListCabinetCourtageGestion = new Models.ModelesRecherche.ModeleRechercheAvancee { Contexte = "courtierGestion" };

                    if (model.AllParameters.TemplateId.GetValueOrDefault() > 0) {
                        CreateCanevas(screenClient);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(id) && id.Contains('_'))
                        {
                            string[] tId = id.Split('_');
                            if (tId.Length < 4) {
                                InitEditOffreCanevas(result, tId[0], tId[1]);
                            }
                            else if (tId.Length == 4) {
                                // Création d'une offre à partir d'un canevas ou d'une autre offre sélectionné en amont
                                if (tId[0].StartsWith("CV")) {
                                    CreateOffreCanevas(tId[0], tId[1]);
                                }
                                else {
                                    CreateOffreCopy(screenClient, tId[0], int.Parse(tId[1]));
                                }
                            }
                            else if (tId.Length >= 6)
                            {
                                // Ouverture pour une copie d'une offre dans une autre offre
                                model.Offre = GetInfoCreationSaisie(tId[3], tId[4]);
                                LoadInfoOffre(model.Offre);
                                LoadListesMotsCles(model.Offre.Branche.Code, model.Offre.Branche.Cible.Code);
                                LoadInfoList(model.Offre, result);
                                model.CodeOffreCopy = tId[0];
                                model.VersionCopy = tId[1];
                                model.CopyMode = true;
                                model.CabinetCourtage.CopyMode = true;
                                model.InformationSaisie.CopyMode = true;
                                model.InformationSaisie.InformationCible.CopyMode = true;
                                model.InformationSaisie.InformationTemplate.CopyMode = true;
                            }
                            model.Navigation = new Navigation_MetaModel();
                            model.Navigation.Etape = Navigation_MetaModel.ECRAN_INFOGENERALE;
                        }
                    }

                    List<AlbSelectListItem> branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Nom), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Nom) }).ToList();
                    branches = branches.Where(el => CacheUserRights.UserRights.Any(elU => (elU.Branche == el.Value || elU.Branche == "**") && elU.TypeDroit != TypeDroit.V.ToString())).ToList();

                    List<AlbSelectListItem> cibles = new List<AlbSelectListItem>();
                    // Si on vient de la recherche en passant les paramètres "Branche"
                    if (this.model.CodeBranche.ContainsChars())
                    {
                        var listBranches = branches.FirstOrDefault(m => m.Value == model.CodeBranche);
                        if (listBranches != null)
                        {
                            listBranches.Selected = true;
                            cibles = GetListeCibles(listBranches.Value);
                        }
                    }

                    model.InformationSaisie.Branches = branches;
                    if (cibles.Count > 0)
                    {
                        model.InformationSaisie.InformationCible.Cibles = cibles;
                    }
                }
            }
            if (model.Offre != null)
            {
                model.InformationSaisie.InformationCible.IsReadOnly = this.model.EditMode && GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version.ToString() + "_" + model.Offre.Type);// || model.InformationSaisie.InformationCible.IsReadOnly;
                model.IsReadOnly = this.model.EditMode && GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre.Trim() + "_" + model.Offre.Version.ToString() + "_" + model.Offre.Type);
                model.Description.IsReadOnly = model.IsReadOnly;
            }
            else
            {
                this.model.IsReadOnly = false;
                this.model.EditMode = false;
                // récupération souscripteur utilisateur  
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var screenClient = channelClient.Channel;
                    var resultSouscripteur = screenClient.SouscripteursGet(new SouscripteursGetQueryDto { NomSouscripteur = GetUser(), DebutPagination = 1, FinPagination = 10 });
                    if (resultSouscripteur.SouscripteursDto.Any())
                    {
                        var souscripteur = resultSouscripteur.SouscripteursDto.FirstOrDefault();
                        model.InformationSaisie.CodeSouscripteur = souscripteur.Code;
                        model.InformationSaisie.Souscripteurs = souscripteur.Code + " - " + souscripteur.Nom;
                    }
                }
                model.Description.MotsClefs1 = new List<AlbSelectListItem>();
                model.Description.MotsClefs2 = new List<AlbSelectListItem>();
                model.Description.MotsClefs3 = new List<AlbSelectListItem>();
                if (model.PreneurAssurance != null) {
                    model.PreneurAssurance.PreneurEstAssure = true;
                }
                if (string.IsNullOrEmpty(model.TypePolicePage)) {
                    model.TypePolicePage = "O";
                }
            }
        }

        private void CreateOffreCopy(ICreationOffre screenClient, string code, int version) {
            string typeOffre = AffaireType.Offre.AsCode();
            var resultBC = screenClient.GetBrancheCibleOffre(code, version.ToString(), typeOffre, model.ModeNavig.ParseCode<ModeConsultation>());
            string branche = resultBC.Split('_')[0];
            string cible = resultBC.Split('_')[1];
            string cibleDesc = resultBC.Split('_')[2];
            LoadListesMotsCles(branche, cible);
            model.Offre = new Offre_MetaModel { CodeOffre = code, Version = version, Type = typeOffre };
            var sourceOffre = GetInfoCreationSaisie(code, version.ToString());
            //var sourceOffre = CacheModels.GetOffreFromCache(tId[0], int.Parse(tId[1]), tId[2], model.ModeNavig.ParseCode<ModeConsultation>());
            if (sourceOffre != null) {
                model.PreneurAssurance.PreneurEstAssure = sourceOffre.PreneurAssurance.PreneurEstAssure;
                model.Description.MotClef1 = sourceOffre.MotCle1;
                if (model.Description.MotsClefs1.Find(elm => elm.Value == sourceOffre.MotCle1) != null)
                    model.Description.MotsClefs1.Find(elm => elm.Value == sourceOffre.MotCle1).Selected = true;

                model.Description.MotClef2 = sourceOffre.MotCle2;
                if (model.Description.MotsClefs2.Find(elm => elm.Value == sourceOffre.MotCle2) != null)
                    model.Description.MotsClefs2.Find(elm => elm.Value == sourceOffre.MotCle2).Selected = true;

                model.Description.MotClef3 = sourceOffre.MotCle3;
                if (model.Description.MotsClefs3.Find(elm => elm.Value == sourceOffre.MotCle3) != null)
                    model.Description.MotsClefs3.Find(elm => elm.Value == sourceOffre.MotCle3).Selected = true;
            }

            model.CopyMode = true;
            model.CabinetCourtage.CopyMode = true;
            model.InformationSaisie.CopyMode = true;
            model.InformationSaisie.Branche = branche;
            model.InformationSaisie.InformationCible.CopyMode = true;
            model.InformationSaisie.InformationTemplate.CopyMode = true;
            model.InformationSaisie.InformationCible.Cible = cible;
            model.InformationSaisie.InformationCible.CibleLibelle = string.Format("{0} - {1}", cible, cibleDesc);

            // récup souscripteur utilisateur
            using (var channelClientPol = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                var screenClientSous = channelClientPol.Channel;
                var resultSouscripteur = screenClientSous.SouscripteursGet(new SouscripteursGetQueryDto { NomSouscripteur = GetUser(), DebutPagination = 1, FinPagination = 10 });
                if (resultSouscripteur.SouscripteursDto.Any()) {
                    var souscripteur = resultSouscripteur.SouscripteursDto.FirstOrDefault();
                    model.InformationSaisie.CodeSouscripteur = souscripteur.Code;
                    model.InformationSaisie.Souscripteurs = souscripteur.Code + " - " + souscripteur.Nom;
                }

                var resultGestionnaire = screenClientSous.GestionnairesGet(new GestionnairesGetQueryDto { NomGestionnaire = GetUser(), DebutPagination = 1, FinPagination = 10 });
                if (resultGestionnaire.GestionnairesDto.Any()) {
                    var gestionnaire = resultGestionnaire.GestionnairesDto.FirstOrDefault();
                    model.InformationSaisie.CodeGestionnaire = gestionnaire.Id;
                    model.InformationSaisie.Gestionnaires = gestionnaire.Id + " - " + gestionnaire.Nom;
                }
            }
        }

        private void CreateOffreCanevas(string code, string version) {
            model.Offre = GetInfoCreationSaisie(code, version);
            LoadDataTemplate(model.Offre, model.Offre.Branche.Code, model.Offre.Branche.Cible.Code, model.Offre.CodeOffre);
            LoadListesMotsCles(model.Offre.Branche.Code, model.Offre.Branche.Cible.Code);
        }

        private void InitEditOffreCanevas(CreationSaisieGetResultDto result, string code, string version) {
            model.Offre = GetInfoCreationSaisie(code, version);
            LoadInfoOffre(model.Offre);
            LoadListesMotsCles(model.Offre.Branche.Code, model.Offre.Branche.Cible.Code);
            LoadInfoList(model.Offre, result);
            model.InformationSaisie.InformationTemplate = null;
        }

        private void CreateCanevas(ICreationOffre screenClient) {
            var infoTemp = screenClient.GetInfoTemplate(model.AllParameters.TemplateId.StringValue());
            model.Offre = ConvertInfoTemplate(infoTemp);

            model.InformationSaisie = new ModeleInformationSaisie {
                Branche = infoTemp.Branche.Code,
                InformationCible = new ModeleInfoSaisieCible { Cible = infoTemp.Branche.Cible.Code, CibleLibelle = infoTemp.Branche.Cible.Code, IsReadOnly = true },
                InformationTemplate = new ModeleInfoSaisieTemplate { Template = infoTemp.CodeOffre },
                EditMode = true
            };
            LoadListesMotsCles(infoTemp.Branche.Code, infoTemp.Branche.Cible.Code);
        }

        private RedirectToRouteResult Redirect(CreationSaisieSetQueryDto query = null, CreationSaisieSetResultDto resultDto = null) {
            if (IsReadonly) {
                if (!string.IsNullOrEmpty(this.model.txtParamRedirect)) {
                    var tabParamRedirect = this.model.txtParamRedirect.Split('/');
                    return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                }
                return RedirectToAction("Index", "ModifierOffre", new { id = this.model.CodeOffre + "_" + this.model.Version + "_" + this.model.Type + GetSurroundedTabGuid(this.model.TabGuid) + this.model.ModeNavig, returnHome = this.model.txtSaveCancel, guidTab = Model.TabGuid });
            }
            else {
                if (this.model.EditMode) {
                    if (!string.IsNullOrEmpty(this.model.txtParamRedirect)) {
                        var tabParamRedirect = this.model.txtParamRedirect.Split('/');
                        return RedirectToAction(tabParamRedirect[1], tabParamRedirect[0], new { id = tabParamRedirect[2] });
                    }
                    return RedirectToAction("Index", "ModifierOffre", new { id = this.model.CodeOffre + "_" + this.model.Version + "_" + this.model.Type + GetSurroundedTabGuid(this.model.TabGuid) + this.model.ModeNavig, returnHome = this.model.txtSaveCancel, guidTab = Model.TabGuid });
                }
                else {
                    InitVerrouillage(resultDto.CodeOffre);
                    return RedirectToAction("Offre", "ConfirmationSaisie", new { id = resultDto.CodeOffre + "_0_O" + GetSurroundedTabGuid(this.model.TabGuid) + this.model.ModeNavig });
                }
            }
        }

        private static Offre_MetaModel GetInfoCreationSaisie(string codeOffre, string version)
        {
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = channelClient.Channel;
                var offreMetaModel = new Offre_MetaModel();
                var resultInfo = serviceContext.GetInfoCreationSaisie(codeOffre, version, AffaireType.Offre.AsCode());

                if (resultInfo == null) return offreMetaModel;

                offreMetaModel.LoadOffre(resultInfo);

                return offreMetaModel;
            }
        }

        private void LoadInfoOffre(Offre_MetaModel offre)
        {
            model.CodeOffre = offre.CodeOffre;
            model.Version = offre.Version;
            model.EditMode = true;
            model.InformationSaisie = new ModeleInformationSaisie
            {
                Branche = offre.Branche.Code,
                Branches = (model.InformationSaisie != null && model.InformationSaisie.Branches != null) ? model.InformationSaisie.Branches : new List<AlbSelectListItem>(),
                InformationCible = new ModeleInfoSaisieCible
                {
                    Cible = offre.Branche.Cible.Code,
                    CibleLibelle = offre.Branche.Cible.Code + " - " + offre.Branche.Cible.Nom,
                    IsReadOnly = true
                },
                EditMode = true
            };

            if (offre.Souscripteur != null)
            {
                model.InformationSaisie.CodeSouscripteur = offre.Souscripteur.Code;
                model.InformationSaisie.Souscripteurs = offre.Souscripteur.Code + " - " + offre.Souscripteur.Nom;
            }

            if (offre.Gestionnaire != null)
            {
                model.InformationSaisie.CodeGestionnaire = offre.Gestionnaire.Id;
                model.InformationSaisie.Gestionnaires = offre.Gestionnaire.Id + " - " + offre.Gestionnaire.Nom;
            }

            if (offre.DateSaisie != null)
            {
                model.InformationSaisie.DateSaisie = new DateTime(offre.DateSaisie.Value.Year, offre.DateSaisie.Value.Month, offre.DateSaisie.Value.Day);
                model.InformationSaisie.HeureSaisieString = new TimeSpan(offre.DateSaisie.Value.Hour, offre.DateSaisie.Value.Minute, offre.DateSaisie.Value.Second);
            }

            model.CabinetCourtage = new ModeleCabinetCourtage
            {
                CodeCabinetCourtage = offre.CabinetGestionnaire.Code,
                NomCabinetCourtage = offre.CabinetGestionnaire.NomCabinet,
                CodeInterlocuteur = offre.CodeInterlocuteur,
                NomInterlocuteur = offre.CodeInterlocuteur == "0" ? string.Empty : offre.CodeInterlocuteur + " - " + offre.NomInterlocuteur,
                Reference = offre.RefChezCourtier,
                EditMode = true
            };
            model.PreneurAssurance = new ModelePreneurAssurance();
            if (offre.PreneurAssurance != null)
            {
                model.PreneurAssurance.CodePreneurAssurance = !string.IsNullOrEmpty(offre.PreneurAssurance.Code) ? Convert.ToInt32(offre.PreneurAssurance.Code) : 0;
                model.PreneurAssurance.PreneurEstAssure = offre.PreneurAssurance.PreneurEstAssure;
            }
            model.PreneurAssurance.NbAssuAdditionnel = offre.NbAssuAdditionnel;

            model.Description = new ModeleDescription
            {
                MotClef1 = offre.MotCle1,
                MotClef2 = offre.MotCle2,
                MotClef3 = offre.MotCle3,
                Descriptif = offre.Descriptif,
                Observation = offre.Observation,
            };

            if (offre.AdresseOffre != null)
            {
                model.ContactAdresse = new ModeleContactAdresse
                {
                    Batiment = offre.AdresseOffre.Batiment,
                    Distribution = offre.AdresseOffre.BoitePostale,
                    Extension = offre.AdresseOffre.ExtensionVoie,
                    MatriculeHexavia = offre.AdresseOffre.MatriculeHexavia,
                    No = offre.AdresseOffre.NumeroVoie == 0 ? string.Empty : offre.AdresseOffre.NumeroVoie.ToString(),
                    No2 = offre.AdresseOffre.NumeroVoie2,
                    NoChrono = offre.AdresseOffre.NumeroChrono,
                    Voie = offre.AdresseOffre.NomVoie,
                    Latitude = offre.AdresseOffre.Latitude?.ToString(),
                    Longitude = offre.AdresseOffre.Longitude?.ToString(),
                    SaisieHexavia = true,
                    FirstIndex = 16
                };
                int depart = 0;
                Int32.TryParse(offre.AdresseOffre.Departement, out depart);

                string codePostal = string.Empty;
                string codePX = string.Empty;
                if (depart > 0)
                {
                    codePostal = depart.ToString("D2") + offre.AdresseOffre.CodePostal.ToString("D3");
                    codePX = depart.ToString("D2") + offre.AdresseOffre.CodePostalCedex.ToString("D3");
                }
                else
                {
                    codePostal = offre.AdresseOffre.CodePostal.ToString();
                    codePX = offre.AdresseOffre.CodePostalCedex.ToString();
                }

                model.ContactAdresse.CodePostal = offre.AdresseOffre.CodePostal == 0 ? string.Empty : offre.AdresseOffre.CodePostal.ToString("D3");
                model.ContactAdresse.CodePostalCedex = offre.AdresseOffre.CodePostalCedex == 0 ? string.Empty : offre.AdresseOffre.CodePostalCedex.ToString("D3");
                model.ContactAdresse.Ville = offre.AdresseOffre.NomVille;
                model.ContactAdresse.VilleCedex = offre.AdresseOffre.NomCedex;
                model.ContactAdresse.Pays = offre.AdresseOffre.NomPays;
                model.ContactAdresse.CodePostal = codePostal;
                model.ContactAdresse.CodePostalCedex = codePX;
            }
            else
            {
                model.ContactAdresse = new ModeleContactAdresse(16, true, true);
            }

            this.model.ContactAdresse.ReadOnly = this.model.EditMode && GetIsReadOnly(GetSurroundedTabGuid(model.TabGuid), model.Offre.CodeOffre + "_" + model.Offre.Version.ToString() + "_" + model.Offre.Type);
        }

        private void LoadInfoList(Offre_MetaModel offre, CreationSaisieGetResultDto result)
        {
            AlbSelectListItem motclef1 = model.Description.MotsClefs1.FirstOrDefault(m => m.Value == offre.MotCle1);
            if (motclef1 != null) motclef1.Selected = true;
            AlbSelectListItem motclef2 = model.Description.MotsClefs2.FirstOrDefault(m => m.Value == offre.MotCle2);
            if (motclef2 != null) motclef2.Selected = true;
            AlbSelectListItem motclef3 = model.Description.MotsClefs3.FirstOrDefault(m => m.Value == offre.MotCle3);
            if (motclef3 != null) motclef3.Selected = true;
        }

        /// <summary>
        /// Fonction qui charge les informations à partir du template en parametre
        /// TODO SLA: ajuster les éléments repris en fonction des specs (lorsque dispo)
        /// </summary>
        /// <param name="offreTemplate"></param>
        private void LoadInfoTemplate(Offre_MetaModel offreTemplate)
        {
            model.CodeOffre = offreTemplate.CodeOffre;
            model.Version = offreTemplate.Version;
            model.EditMode = false;
            model.CopyMode = true;
            model.CodeOffreCopy = offreTemplate.CodeOffre;
            model.VersionCopy = offreTemplate.Version.Value.ToString();
            model.PreneurAssurance.PreneurEstAssure = true;
            model.InformationSaisie = new ModeleInformationSaisie
            {
                Branche = offreTemplate.Branche.Code,
                InformationCible = new ModeleInfoSaisieCible { Cible = offreTemplate.Branche.Cible.Code, CibleLibelle = offreTemplate.Branche.Cible.Code, IsReadOnly = true },
                InformationTemplate = new ModeleInfoSaisieTemplate { CopyMode = true, IsReadOnly = true, Template = offreTemplate.CodeOffre },
                EditMode = false,
                CopyMode = true
            };

            #region chargement des données complémentaires supprimées après obtention SPEC
            //if (offreTemplate.Souscripteur != null)
            //{
            //    model.InformationSaisie.CodeSouscripteur = offreTemplate.Souscripteur.Code;
            //    model.InformationSaisie.Souscripteurs = offreTemplate.Souscripteur.Code + " - " + offreTemplate.Souscripteur.Nom;
            //}

            //if (offreTemplate.Gestionnaire != null)
            //{
            //    model.InformationSaisie.CodeGestionnaire = offreTemplate.Gestionnaire.Id;
            //    model.InformationSaisie.Gestionnaires = offreTemplate.Gestionnaire.Id + " - " + offreTemplate.Gestionnaire.Nom;
            //}

            model.CabinetCourtage = new ModeleCabinetCourtage
            {
                CodeCabinetCourtage = offreTemplate.CabinetApporteur.Code,
                NomCabinetCourtage = offreTemplate.CabinetApporteur.NomCabinet,
                CodeInterlocuteur = offreTemplate.CodeInterlocuteur,
                NomInterlocuteur = offreTemplate.CodeInterlocuteur == "0" ? string.Empty : offreTemplate.CodeInterlocuteur + " - " + offreTemplate.NomInterlocuteur,
                Reference = offreTemplate.RefChezCourtier,
                EditMode = false
            };
            model.PreneurAssurance = new ModelePreneurAssurance();
            if (offreTemplate.PreneurAssurance != null)
            {
                model.PreneurAssurance.CodePreneurAssurance = !string.IsNullOrEmpty(offreTemplate.PreneurAssurance.Code) ? Convert.ToInt32(offreTemplate.PreneurAssurance.Code) : 0;
                model.PreneurAssurance.NomPreneurAssurance = offreTemplate.PreneurAssurance.NomAssure;
                if (offreTemplate.PreneurAssurance.Adresse != null && offreTemplate.PreneurAssurance.Adresse.NomVille != null)
                {
                    model.PreneurAssurance.Departement = offreTemplate.PreneurAssurance.Adresse.CodePostalString;
                    model.PreneurAssurance.Ville = offreTemplate.PreneurAssurance.Adresse.NomVille;
                }
            }

            //if (offreTemplate.AdresseOffre != null)
            //{
            //    model.ContactAdresse = new ModeleContactAdresse
            //    {
            //        Batiment = offreTemplate.AdresseOffre.Batiment,
            //        Distribution = offreTemplate.AdresseOffre.BoitePostale,
            //        Extension = offreTemplate.AdresseOffre.ExtensionVoie,
            //        MatriculeHexavia = offreTemplate.AdresseOffre.MatriculeHexavia,
            //        No = offreTemplate.AdresseOffre.NumeroVoie,
            //        NoChrono = offreTemplate.AdresseOffre.NumeroChrono,
            //        Voie = offreTemplate.AdresseOffre.NomVoie,
            //        //ReadOnly = true,
            //        SaisieHexavia = true,
            //        FirstIndex = 16
            //    };

            //    if (offreTemplate.AdresseOffre.Ville != null)
            //    {
            //        model.ContactAdresse.CodePostal = offreTemplate.AdresseOffre.Ville.CodePostal;
            //        model.ContactAdresse.CodePostalCedex = offreTemplate.AdresseOffre.Ville.CodePostalCedex;
            //        model.ContactAdresse.Ville = offreTemplate.AdresseOffre.Ville.Nom;
            //        model.ContactAdresse.VilleCedex = offreTemplate.AdresseOffre.Ville.NomCedex;
            //    }
            //    if (offreTemplate.AdresseOffre.PaysDto != null)
            //    {
            //        model.ContactAdresse.Pays = offreTemplate.AdresseOffre.PaysDto.Nom;
            //    }
            //}
            //else
            //{
            //    model.ContactAdresse = new ModeleContactAdresse(16, true, true);
            //}
            #endregion

            model.Description = new ModeleDescription
            {
                MotClef1 = offreTemplate.MotCle1,
                MotClef2 = offreTemplate.MotCle2,
                MotClef3 = offreTemplate.MotCle3,
                Observation = offreTemplate.Observation
                //  Descriptif = offreTemplate.Descriptif //SLA 21.05.15 : desactivé sur demande mail jda 21.05.2015 à zbo
            };

            LoadListesMotsCles(model.Offre.Branche.Code, model.Offre.Branche.Cible.Code);
            SetSelectedItem(model.Description.MotsClefs1, model.Description.MotClef1);
            SetSelectedItem(model.Description.MotsClefs2, model.Description.MotClef2);
            SetSelectedItem(model.Description.MotsClefs3, model.Description.MotClef3);


        }

        private Offre_MetaModel ConvertInfoTemplate(OffreDto offreDto)
        {
            Offre_MetaModel offre = new Offre_MetaModel
            {
                CodeOffre = offreDto.CodeOffre,
                Version = offreDto.Version,
                Type = offreDto.Type,
                Branche = new BrancheDto
                {
                    Code = offreDto.Branche.Code,
                    Cible = new CibleDto
                    {
                        Code = offreDto.Branche.Cible.Code
                    }
                }
            };

            return offre;
        }

        private List<AlbSelectListItem> GetListeCibles(string codeBranche)
        {
            List<AlbSelectListItem> toReturn = null;
            if (!string.IsNullOrEmpty(codeBranche))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
                {
                    var serviceContext = channelClient.Channel;
                    var result = serviceContext.GetCibles(codeBranche, false, CacheUserRights.UserRights.Any(
                el => el.TypeDroit == TypeDroit.A.ToString() || el.TypeDroit == TypeDroit.M.ToString()), MvcApplication.USER_HORSE.IndexOf(GetUser()) > 0);
                    if (result != null)
                    {
                        toReturn = result.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Libelle), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Libelle) }).ToList(); ;
                    }
                }
            }
            if (toReturn == null)
                toReturn = new List<AlbSelectListItem>();
            return toReturn;
        }

        private List<AlbSelectListItem> GetListeTemplates(string codeBranche, string codeCible)
        {
            List<AlbSelectListItem> toReturn = null;
            if (!string.IsNullOrEmpty(codeBranche) && !string.IsNullOrEmpty(codeCible))
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var serviceContext = channelClient.Channel;
                    var result = serviceContext.LoadListTemplates(0, string.Empty, string.Empty, AlbConstantesMetiers.TYPE_OFFRE, codeCible, codeBranche, false, true);
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

        private List<AlbSelectListItem> LstMotsCles(string codeBranche, string codeCible)
        {
            var value = new List<AlbSelectListItem>();
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var serviceContext = channelClient.Channel;
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

        private ActionResult LoadDataTemplate(Offre_MetaModel template, string codeBranche, string codeCible, string codeTemplate)
        {
            if (template != null)
            {
                model.Offre = template;
                LoadInfoTemplate(model.Offre);
            }
            else
            {
                model.Description = new ModeleDescription();
                LoadListesMotsCles(codeBranche, codeCible);
            }

            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICreationOffre>())
            {
                var screenClient = channelClient.Channel;
                var query = new CreationSaisieGetQueryDto();
                var result = screenClient.CreationSaisieGet(query);



                if (result != null)
                {
                    List<AlbSelectListItem> branches = result.Branches.Select(m => new AlbSelectListItem { Value = m.Code, Text = string.Format("{0} - {1}", m.Code, m.Nom), Selected = false, Title = string.Format("{0} - {1}", m.Code, m.Nom) }).ToList();
                    branches = branches.Where(el => CacheUserRights.UserRights.Any(elU => (elU.Branche == el.Value || elU.Branche == "**") && elU.TypeDroit != TypeDroit.V.ToString())).ToList();

                    model.InformationSaisie.Branches = branches;
                }

                model.InformationSaisie.Branche = codeBranche;
                model.InformationSaisie.InformationCible.Cible = codeCible;

                model.InformationSaisie.InformationCible.Cibles = GetListeCibles(model.InformationSaisie.Branche);
                if (model.InformationSaisie.InformationCible.Cibles.Find(elm => elm.Value == model.InformationSaisie.InformationCible.Cible) != null)
                    model.InformationSaisie.InformationCible.Cibles.Find(elm => elm.Value == model.InformationSaisie.InformationCible.Cible).Selected = true;

                model.InformationSaisie.InformationTemplate.Templates = GetListeTemplates(model.InformationSaisie.Branche, model.InformationSaisie.InformationCible.Cible);
                if (model.InformationSaisie.InformationTemplate.Templates.Find(elm => elm.Value.Trim() == (string.IsNullOrEmpty(model.InformationSaisie.InformationTemplate.Template) ? string.Empty : model.InformationSaisie.InformationTemplate.Template.Trim())) != null)
                {
                    model.InformationSaisie.InformationTemplate.Templates.ForEach(elm => elm.Selected = false);
                    model.InformationSaisie.InformationTemplate.Templates.Find(elm => elm.Value.Trim() == model.InformationSaisie.InformationTemplate.Template.Trim()).Selected = true;
                }

                model.InformationSaisie.InformationTemplate.IsReadOnly = false;

                model.LoadTemplateMode = true;
                model.InformationSaisie.LoadTemplateMode = true;
                model.InformationSaisie.InformationCible.LoadTemplateMode = true;
                model.InformationSaisie.InformationTemplate.LoadTemplateMode = true;
            }

            if (string.IsNullOrEmpty(codeTemplate))
            {
                model.InformationSaisie.InformationTemplate.Templates.ForEach(elm => elm.Selected = false);
                model.CopyMode = false;
            }

            //récup souscripteur utilisateur               
            using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>())
            {
                var screenClient = channelClient.Channel;
                var resultSouscripteur = screenClient.SouscripteursGet(new SouscripteursGetQueryDto { NomSouscripteur = GetUser(), DebutPagination = 1, FinPagination = 10 });
                if (resultSouscripteur.SouscripteursDto.Any())
                {
                    var souscripteur = resultSouscripteur.SouscripteursDto.FirstOrDefault();
                    model.InformationSaisie.CodeSouscripteur = souscripteur.Code;
                    model.InformationSaisie.Souscripteurs = souscripteur.Code + " - " + souscripteur.Nom;
                }

                var resultGestionnaire = screenClient.GestionnairesGet(new GestionnairesGetQueryDto { NomGestionnaire = GetUser(), DebutPagination = 1, FinPagination = 10 });
                if (resultGestionnaire.GestionnairesDto.Any())
                {
                    var gestionnaire = resultGestionnaire.GestionnairesDto.FirstOrDefault();
                    model.InformationSaisie.CodeGestionnaire = gestionnaire.Id;
                    model.InformationSaisie.Gestionnaires = gestionnaire.Id + " - " + gestionnaire.Nom;
                }
            }

            model.PreneurAssurance.PreneurEstAssure = true;

            //Indique que nous ne sommes pas entrain de créer un canevas
            ViewData["IsCanevas"] = false;

            return PartialView("BodyCreationSaisie", model);
        }

        private void LoadListesMotsCles(string codeBranche, string codeCible)
        {
            if (model.Description != null)
            {
                model.Description.MotsClefs1 = LstMotsCles(codeBranche, codeCible);
                model.Description.MotsClefs2 = LstMotsCles(codeBranche, codeCible);
                model.Description.MotsClefs3 = LstMotsCles(codeBranche, codeCible);
            }
        }

        private static void SetSelectedItem(List<AlbSelectListItem> lookUp, string compareValue, AlbSelectListItem selectedItem = null)
        {
            if (!string.IsNullOrEmpty(compareValue) && lookUp != null)
            {
                selectedItem = lookUp.FirstOrDefault(elm => elm.Text.Split('-')[0].Trim() == compareValue.Trim());
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }
        }

        /// <summary>
        /// Set la valeur d'une propriété du ViewBag permettant d'identifier si l'offre en cours d'utilisation est un Canevas
        /// </summary>
        private void SetViewBagCanevas()
        {
            if (model.Offre != null && model.Offre.CodeOffre != null)
            {
                ViewData["IsCanevas"] = model.Offre.CodeOffre.Trim().Substring(0, 2) == "CV" ? true : false;
            }
            else
            {
                ViewData["IsCanevas"] = false;
            }
        }

        /// <summary>
        /// Vérification partenaires / offre
        /// </summary>
        /// <param name="model">offre</param>
        private void VerificationPartenairesOffre(ModeleCreationSaisiePage model)
        {
            //T 3997 : Vérification des partenaires
            if (model != null && CanUseLablat())
            {
                using (var channelClient = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>())
                {
                    var partenaires = new PartenairesDto
                    {
                        CourtierGestionnaire = new PartenaireDto
                        {
                            Code = model.CabinetCourtage?.CodeCabinetCourtage.ToString(),
                            Nom = model.CabinetCourtage.NomCabinetCourtage,
                            CodeInterlocuteur = model.CabinetCourtage?.CodeInterlocuteur.ParseInt(),
                            NomInterlocuteur = model.CabinetCourtage?.NomInterlocuteur
                        },

                        PreneurAssurance = new PartenaireDto
                        {
                            Code = model.PreneurAssurance?.CodePreneurAssurance.ToString(),
                            Nom = model.PreneurAssurance?.NomPreneurAssurance
                        },
                        AssuresAdditionnels = channelClient.Channel.GetListAssuresAdditionnelsInfosBase(
                                                             model.CodeOffre.Trim(),
                                                             model.Version.ToString(),
                                                             model.Type,
                                                             "0",
                                                             ModeConsultation.Standard
                                                            )
                    };
                    VerificationPartenaires(partenaires);
                }
            }
        }
        #endregion

    }
}

