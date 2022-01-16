using Albingia.Common;
using Albingia.Kheops.Mvc.Models;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Business;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.ServiceFactory;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Controllers.AffaireNouvelle;
using ALBINGIA.OP.OP_MVC.Controllers.Avenant;
using ALBINGIA.OP.OP_MVC.Controllers.Regularisation;
using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage;
using ALBINGIA.OP.OP_MVC.Models.MetaData;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesNavigationArbre;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.AffaireNouvelle;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Common;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Partenaire;
using OPServiceContract;
using OPServiceContract.IAdministration;
using OPServiceContract.IAffaireNouvelle;
using OPServiceContract.ICommon;
using OPServiceContract.ISaisieCreationOffre;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Controllers {
    /// <summary>
    /// Classe de base dont doivent hériter tous les controleurs
    /// </summary>
    [AlbDetectBrowser]
    public class ControllersBase<T> : BaseController, IMetaModelsController where T : MetaModelsBase {
        protected ControllersBase() {
            model = Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Gets the Model of type <see cref="T"/>
        /// </summary>
        protected readonly T model;

        public MetaModelsBase Model { get => model; }

        /// <summary>
        /// Login de l'utilisateur connecté
        /// </summary>
        public string UserId {
            get { return AlbSessionHelper.ConnectedUser; }
        }
        /// <summary>
        /// Taille de la page
        /// </summary>
        public int PageSize {
            get { return MvcApplication.PAGINATION_PAGE_SIZE; }
        }

        virtual public bool IsReadonly {
            get {
                Guid guid = Guid.TryParse(Model.TabGuid, out var g) ? g : default;
                return GetState(guid, new Albingia.Kheops.OP.Domain.Affaire.AffaireId {
                    CodeAffaire = Model.CodePolicePage,
                    NumeroAliment = int.TryParse(Model.VersionPolicePage, out var v) ? v : default
                }) == ControllerState.Readonly;
            }
        }

        public bool IsModifHorsAvenant {
            get {
                Guid guid = Guid.TryParse(Model.TabGuid, out var g) ? g : default;
                return GetState(guid, new Albingia.Kheops.OP.Domain.Affaire.AffaireId {
                    CodeAffaire = Model.CodePolicePage,
                    NumeroAliment = int.TryParse(Model.VersionPolicePage, out var v) ? v : default
                }) == ControllerState.PartialEdit;
            }
        }

        virtual public bool? IsAvnDisabled { get;}

        virtual public bool AllowUpdate => !IsReadonly && (IsModifHorsAvenant || !IsAvnDisabled.GetValueOrDefault());

        static readonly IEnumerable<Type> BackOfficeControllers = new HashSet<Type> {
            typeof(BackOfficeController),
            typeof(__ClausesController),
            typeof(VoletsController), typeof(BlocsController), typeof(AssocierVBMController), typeof(GarantieModeleController), typeof(GarantieTypeController),
            typeof(ParamClauseController), typeof(CopyParamClauseController), typeof(LibellesClausesController),
            typeof(ParamCibleController), typeof(ParamCibleRecupController),
            typeof(ParamInventairesController),
            typeof(ParamEngagementsController), typeof(FraisAccessoiresController),
            typeof(ParamConceptsController), typeof(ParamFamillesController), typeof(ParamFiltresController), typeof(ParamTypesValeurController),
            typeof(ParamGarantiesController),
            typeof(ParamTemplatesController),
            typeof(ParamISReferenceController), typeof(ParamISModelesController), typeof(ParamISModeleAssocierController),
            typeof(RefExprCompController),
            typeof(GestionNomenclatureController), typeof(GestionGrilleNomenclatureController),
            typeof(ParamDocumentCheminController),
            typeof(OffresVerrouilleesController), typeof(GestUtilisateursController), typeof(StatistiqueController), typeof(StatClausesLibresController),
            typeof(LogTraceController), typeof(LogPerfController),
            typeof(InformationsDataBaseController),
            typeof(RechercheOffreRapideController),
            typeof(SuspensionController),
            typeof(ScriptController)
        };

        static readonly IDictionary<Type, string[]> AllowedActionResultsFromAjaxCall = new Dictionary<Type, string[]> {
            { typeof(ModifierOffreController), new[] { nameof(ModifierOffreController.Index) } },
            { typeof(AnCreationContratController), new[] { nameof(AnCreationContratController.Index) } },
            { typeof(AnInformationsGeneralesController), new[] { nameof(AnInformationsGeneralesController.Index) } },
            { typeof(AvenantInfoGeneralesController), new[] { nameof(AvenantInfoGeneralesController.Index) } },
            { typeof(CreationAffaireNouvelleController), new[] { nameof(CreationAffaireNouvelleController.Index), nameof(CreationAffaireNouvelleController.ChoixRisques) } },
            { typeof(DoubleSaisieController), new[] { nameof(DoubleSaisieController.Index) } },
            { typeof(CreationAvenantController), new[] { nameof(CreationAvenantController.Index) } },
            { typeof(ConfirmationSaisieController), new[] { nameof(ConfirmationSaisieController.Offre) } },
            { typeof(EngagementPeriodesController), new[] { nameof(EngagementPeriodesController.Index) } },
            { typeof(CreationRegularisationController), new[] { nameof(CreationRegularisationController.Step1_ChoixPeriode) } },
            { typeof(RetoursController), new[] { nameof(RetoursController.OpenRetours) } }
        };

        public bool IsBackOfficeContext => BackOfficeControllers.Contains(GetType());

        /// <summary>
        /// Taille de la page pour suivi doc
        /// </summary>
        public int PageSizeDoc {
            get { return MvcApplication.PAGINATION_PAGE_SIZE_DOC; }
        }

        [HttpPost]
        public virtual PartialViewResult ReloadNavigationArbre(T modelT, string etape, int codeRisque = 0, int codeFormule = 0, int numOption = 0) {
            bool isOffre = modelT.TypePolicePage == AlbConstantesMetiers.TYPE_OFFRE;
            return PartialView(
                $"/Views/Shared/LayoutArbres/LayoutArbre{(isOffre ? string.Empty : "Contrat")}.cshtml",
                isOffre ? GetNavigationArbre(etape, codeRisque, codeFormule, numOption) : GetNavigationArbreAffaireNouvelle(etape, codeRisque, codeFormule));
        }

        public static bool IsActionResultAjaxGetAllowed(Controller controller, string action) {
            if (controller == null) {
                throw new ArgumentNullException(nameof(controller));
            }
            if (action.IsEmptyOrNull()) {
                throw new ArgumentNullException(nameof(action));
            }
            if (AllowedActionResultsFromAjaxCall.TryGetValue(controller.GetType(), out string[] actions)) {
                return actions.Any(a => a.ToUpper().Trim() == action.ToUpper().Trim());
            }
            return false;
        }

        protected override ContentResult Content(string content, string contentType, System.Text.Encoding contentEncoding) {
            return base.Content(content.Replace("\t", string.Empty), contentType, contentEncoding);
        }

        protected virtual bool GetIsReadOnly(string guid, Folder currentFolder, bool isPopup = false, string modeAvenant = "") {
            return GetIsReadOnly(guid, currentFolder.Identifier, currentFolder.NumeroAvenant.ToString(), isPopup, modeAvenant);
        }
        protected override bool GetIsReadOnly(string guid, string currentFolder, string numAvenant = "0", bool isPopup = false, string modeAvenant = "") {
            if (this.model.TabGuid != guid) {
                this.model.TabGuid = guid.Replace(PageParamContext.TabGuidKey, string.Empty);
            }
            if (this.model.CodePolicePage.IsEmptyOrNull()) {
                this.model.CodePolicePage = currentFolder.Split('_').FirstOrDefault();
            }
            if (this.model.VersionPolicePage.IsEmptyOrNull()) {
                this.model.VersionPolicePage = currentFolder.Split('_').Skip(1).FirstOrDefault();
            }
            return IsReadonly;
        }

        public static void SetGuid(MetaModelsBase model, string id, out string outId) {
            model.AllParameters = AlbParameters.Parse(id);
            outId = model.AllParameters.FolderId;
        }

        public bool GetIsModifHorsAvn(string guid, string folder) {
            if (this.model.TabGuid != guid) {
                this.model.TabGuid = guid.Replace(PageParamContext.TabGuidKey, string.Empty);
            }
            if (this.model.CodePolicePage.IsEmptyOrNull()) {
                this.model.CodePolicePage = folder.Split('_').FirstOrDefault();
            }
            if (this.model.VersionPolicePage.IsEmptyOrNull()) {
                this.model.VersionPolicePage = folder.Split('_').Skip(1).FirstOrDefault();
            }
            return IsModifHorsAvenant;
        }

        internal bool HasEngCnx() {
            bool existEngCnx = false;
            using (var client = ServiceClientFactory.GetClient<IFinOffre>()) {
                string numConnexite = client.Channel.GetNumeroConnexite(Model.CodePolicePage, Model.VersionPolicePage, Model.TypePolicePage, "20");
                if (!string.IsNullOrEmpty(numConnexite)) {
                    var contratsConnexes = client.Channel.GetContratsConnexes(Model.TypePolicePage, "20", numConnexite);
                    existEngCnx = contratsConnexes?.Any() ?? false;
                }
            }
            return existEngCnx;
        }

        internal bool HasCatnatBase() {
            bool hasCatnatBase = false;
            using (var client = ServiceClientFactory.GetClient<IAffairePort>()) {
                var affaire = client.Channel.GetAffaire(new AffaireId {
                    CodeAffaire = Model.CodePolicePage,
                    NumeroAliment = int.Parse(Model.VersionPolicePage),
                    TypeAffaire = Model.TypePolicePage.ParseCode<AffaireType>(),
                    NumeroAvenant = int.TryParse(Model.NumAvenantPage, out int a) && a >= 0 ? a : default,
                    IsHisto = Model.ModeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique
                });
                hasCatnatBase = affaire.BaseCATNATCalculee > decimal.Zero;
            }
            return hasCatnatBase;
        }

        /// <summary>
        /// Retourne true si mode consultation (hors force et ignore readonly)
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="currentFolder"></param>
        /// <returns></returns>
        public static bool GetIsReadOnlyScreen(string guid = "", string currentFolder = "") {
            return AlbTransverse.GetIsReadOnlyScreen(guid, currentFolder);
        }

        public static string GetFormatModeNavig(string modeNavig) {
            return string.Format("modeNavig{0}modeNavig", modeNavig);
        }

        protected virtual ActionResult Init(string id) {
            if (Request.IsAjaxRequest() && !IsActionResultAjaxGetAllowed(this, ControllerContext.RouteData.Values["action"] as string)) {
                //throw new InvalidOperationException("Unappropirate use of Init function. Request should not be AJAX");
            }
            id = FormatId(id);
            SetPageTitle();
            string context = InitializeParams(id);
            LoadInfoPage(context);
            UpdateModel();
            return View(model);
        }

        protected virtual string FormatId(string id) {
            return id;
        }

        protected virtual void SetPageTitle() { }

        protected virtual string InitializeParams(string id, bool strictMode = true) {
            if (id.IsEmptyOrNull()) { return null; }

            model.AllParameters = AlbParameters.Parse(id, strictMode);

            string context = model.AllParameters.FolderId;

            if (this.model.AllParameters[PipedParameter.GUIDKEY].ContainsChars() && this.model.TabGuid.IsEmptyOrNull()) {
                this.model.TabGuid = this.model.AllParameters[PipedParameter.GUIDKEY];
            }

            model.AddParamValue = model.AllParameters.BuildParamsString();

            var folder = this.model.AllParameters.Folder;
            if (folder != null) {
                model.CodePolicePage = folder.CodeOffre;
                model.VersionPolicePage = folder.Version.ToString();
                model.TypePolicePage = folder.Type;
            }

            SetScreenType();

            SetActeGestion(model.AllParameters.Folder?.Type ?? string.Empty);

            if (model.NumAvenantPage.IsEmptyOrNull()) {
                model.NumAvenantPage = default(int).ToString();
            }

            if (this.model.AllParameters[AlbParameters.ConsultOnlyAndEditKey].AsBoolean().GetValueOrDefault()) {
                this.model.IsModifHorsAvenant = true;
            }

            return context;
        }

        protected virtual void LoadInfoPage(string context) {

        }

        protected virtual void UpdateModel() {

        }

        protected static string BuildAddParamString(string typeParam, string valueParam) {
            if (!string.IsNullOrEmpty(typeParam) && !string.IsNullOrEmpty(valueParam)) {
                return $"{PageParamContext.ParamKey}{typeParam}|||{valueParam}{PageParamContext.ParamKey}";
            }
            return string.Empty;
        }

        protected static void SetBlocageInfoMessage(Bandeau_MetaData bandeau, string stopContentieuxInfo, string stopCodeInfo) {
            bool istopCode;
            bool iStopContentieux;
            int stopCode;
            int stopContentieux;

            istopCode = int.TryParse(stopCodeInfo, out stopCode);
            iStopContentieux = int.TryParse(stopContentieuxInfo, out stopContentieux);


            if (istopCode || iStopContentieux) {
                if (stopCode >= 1 || stopContentieux >= 1) {
                    bandeau.Displaybloqueferme = true;
                }
                else {
                    bandeau.Displaybloqueferme = false;
                }
            }
            else {
                bandeau.Displaybloqueferme = false;
            }
        }

        protected static string GetUser() {
            return AlbSessionHelper.ConnectedUser;
        }

        protected static bool IsModuleGestDocOpen() {
            return MvcApplication.SWITCH_MODULE_GESTDOC == 0
                && MvcApplication.USER_MODULE_GESTDOC_OPEN.ToLower().Contains(AlbSessionHelper.ConnectedUser.ToLower())
                && !MvcApplication.USER_MODULE_GESTDOC_CLOSE.ToLower().Contains(AlbSessionHelper.ConnectedUser.ToLower())
                || MvcApplication.SWITCH_MODULE_GESTDOC == 1;
        }

        protected ModeleNavigationArbre GetNavigationArbre(string etape, int codeRisque = 0, int codeFormule = 0, int numOption = 0, bool returnEmptyTree = false) {
            model.NavigationArbre = new ModeleNavigationArbre();
            if (Model.Offre != null && !returnEmptyTree) {
                var folder = new Folder(this.model.Offre.CodeOffre.ToIPB(), (int)this.model.Offre.Version, this.model.Offre.Type[0]) {
                    NumeroAvenant = model.NumAvenantPage.ParseInt().Value
                };
                Model.NavigationArbre = BuildNavigationArbre(folder);
            }
            if (this.model.Contrat != null) {
                using (var client = ServiceClientFactory.GetClient<ICommonAffaire>()) {
                    if (this.model.NumAvenantPage.ParseInt().Value > 0) {
                        var alertesAvenant = client.Channel.GetListAlertesAvenant(new AffaireId {
                            CodeAffaire = this.model.Contrat.CodeContrat,
                            IsHisto = this.model.ModeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique,
                            NumeroAliment = (int)this.model.Contrat.VersionContrat,
                            TypeAffaire = AffaireType.Contrat,
                            NumeroAvenant = this.model.NumAvenantPage.ParseInt().Value
                        });
                        this.model.NavigationArbre.AlertesAvenant = CreationAvenantController.GetInfoAlertes(new AvenantDto { Alertes = alertesAvenant });
                    }
                    else {
                        this.model.NavigationArbre.AlertesAvenant = new List<Models.ModelesCreationAvenant.ModeleAvenantAlerte>();
                    }
                }
            }

            model.NavigationArbre.Etape = etape;
            model.NavigationArbre.CodeRisque = codeRisque;
            model.NavigationArbre.CodeFormule = codeFormule;
            model.NavigationArbre.CodeOption = numOption;
            return model.NavigationArbre;
        }

        protected ModeleNavigationArbre GetNavigationArbreAffaireNouvelle(string etape, int codeRisque = 0, int codeFormule = 0, bool returnEmptyTree = false, bool isTransverseAllowed = false) {
            model.NavigationArbre = new ModeleNavigationArbre();

            var regule = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
            model.NavigationArbre.IsRegule = regule == AlbConstantesMetiers.TYPE_AVENANT_REGULMODIF || regule == AlbConstantesMetiers.TYPE_AVENANT_REGUL;

            bool isCheckedEch = false;
            if (model.Contrat != null && model.Contrat.CodeContrat != null && !returnEmptyTree) {
                var folder = new Folder(this.model.Contrat.CodeContrat, (int)this.model.Contrat.VersionContrat, this.model.Contrat.Type[0]) {
                    NumeroAvenant = int.TryParse(this.model.NumAvenantPage, out int i) && i > 0 ? i : default
                };
                Model.NavigationArbre = BuildNavigationArbre(folder, isRegule: model.NavigationArbre.IsRegule);
                isCheckedEch = this.model.Contrat.IsCheckedEcheance;
                using (var client = ServiceClientFactory.GetClient<ICommonAffaire>()) {
                    if (this.model.NumAvenantPage.ParseInt().Value > 0) {
                        var alertesAvenant = client.Channel.GetListAlertesAvenant(new AffaireId {
                            CodeAffaire = this.model.Contrat.CodeContrat,
                            IsHisto = this.model.ModeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique,
                            NumeroAliment = (int)this.model.Contrat.VersionContrat,
                            TypeAffaire = AffaireType.Contrat,
                            NumeroAvenant = this.model.NumAvenantPage.ParseInt().Value
                        });
                        this.model.NavigationArbre.AlertesAvenant = CreationAvenantController.GetInfoAlertes(new AvenantDto { Alertes = alertesAvenant });
                    }
                    else {
                        this.model.NavigationArbre.AlertesAvenant = new List<Models.ModelesCreationAvenant.ModeleAvenantAlerte>();
                    }
                }
            }

            model.NavigationArbre.IsEmptyRequested = returnEmptyTree;
            model.NavigationArbre.IsTransverseAllowed = isTransverseAllowed;
            model.NavigationArbre.Etape = etape;
            model.NavigationArbre.CodeRisque = codeRisque;
            model.NavigationArbre.CodeFormule = codeFormule;
            model.NavigationArbre.IsCheckedEcheance = isCheckedEch;
            return model.NavigationArbre;
        }

        protected ModeleNavigationArbre BuildNavigationArbre(Folder folder = null, MetaModelsBase modelBase = null, bool isRegule = false) {
            if (folder is null) {
                folder = new Folder((modelBase ?? Model).CodePolicePage, int.Parse((modelBase ?? Model).VersionPolicePage), (modelBase ?? Model).TypePolicePage[0]) {
                    NumeroAvenant = int.TryParse((modelBase ?? Model).NumAvenantPage, out int i) && i > 0 ? i : default
                };
            }
            using (var channelClient = ServiceClientFactory.GetClient<INavigationService>()) {
                var arbreDto = channelClient.Channel.GetTreeHierarchy(folder, IsModifHorsAvenant, this.model.ModeNavig.ParseCode<ModeConsultation>());
                var arbre = (ModeleNavigationArbre)arbreDto;
                arbre.ScreenType = (modelBase ?? Model).ScreenType;
                arbre.IsReadOnly = IsReadonly || IsModifHorsAvenant;
                arbre.ModeNavig = (modelBase ?? Model).ModeNavig;
                arbre.IsValidation = (modelBase ?? Model).IsValidation;
                arbre.NumAvn = folder.NumeroAvenant;
                arbreDto.NumAvn = folder.NumeroAvenant;
                arbre.BuildLinks(arbreDto, isRegule);
                return arbre;
            }
        }

        protected virtual ModeleNavigationArbre GetNavigationArbreRegule(MetaModelsBase contentData, string etape) {
            contentData.NavigationArbre = new ModeleNavigationArbre();
            if (contentData.Contrat != null && contentData.Contrat.CodeContrat != null) {
                var folder = new Folder(contentData.Contrat.CodeContrat, (int)contentData.Contrat.VersionContrat, contentData.Contrat.Type[0]) {
                    NumeroAvenant = int.TryParse(contentData.NumAvenantPage, out int i) && i > 0 ? i : default
                };
                contentData.NavigationArbre = BuildNavigationArbre(folder, contentData);

                using (var client = ServiceClientFactory.GetClient<ICommonAffaire>()) {
                    if (contentData.NumAvenantPage.ParseInt().Value > 0) {
                        var alertesAvenant = client.Channel.GetListAlertesAvenant(new AffaireId {
                            CodeAffaire = contentData.Contrat.CodeContrat,
                            IsHisto = contentData.ModeNavig.ParseCode<ModeConsultation>() == ModeConsultation.Historique,
                            NumeroAliment = (int)contentData.Contrat.VersionContrat,
                            TypeAffaire = AffaireType.Contrat,
                            NumeroAvenant = contentData.NumAvenantPage.ParseInt().Value
                        });
                        contentData.NavigationArbre.AlertesAvenant = CreationAvenantController.GetInfoAlertes(new AvenantDto { Alertes = alertesAvenant });
                    }
                }
            }

            contentData.NavigationArbre.Etape = etape;
            ExtendNavigationArbreRegule(contentData);
            return contentData.NavigationArbre;
        }

        protected virtual void ExtendNavigationArbreRegule(MetaModelsBase contentData) {
            using (var client = ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                var serviceContext = client.Channel;
                var lotId = GetAddParamValue(contentData.AddParamValue, AlbParameterName.LOTID);
                var matrice = serviceContext.GetRegulMatrice(contentData.CodePolicePage, Convert.ToInt32(contentData.VersionPolicePage), contentData.TypePolicePage, lotId);

                var isMonoRisque = matrice.Select(m => m.RisqueId).Distinct().Count() == 1;
                var isMonoGar = isMonoRisque && matrice.Select(m => m.GarId).Distinct().Count() == 1;

                contentData.NavigationArbre.IsMonoRisque = isMonoRisque;
                contentData.NavigationArbre.IsMonoGarantie = isMonoGar;
            }
        }

        protected void SetAllGenParam(string id, out string outId) {
            //Traitement de la consultation en mode histo ou standard
            model.ModeNavig = id.Contains("modeNavig") ?
                id.Split(new[] { "modeNavig" }, StringSplitOptions.None)[1] :
                ModeConsultation.Standard.AsCode();

            id = id.Contains("modeNavig") ? id.Split(new[] { "modeNavig" }, StringSplitOptions.None)[0] : id;

            //Traitement du tabGuid
            model.TabGuid = id.Contains("tabGuid") ? id.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1] : string.Empty;
            outId = id.Contains("tabGuid") ? id.Split(new[] { "tabGuid" }, StringSplitOptions.None)[0] : id;

            //Traitement du paramètre additionnel
            string addParam = id.Contains("addParam") ? id.Split(new[] { "addParam" }, StringSplitOptions.None)[1] : string.Empty;
            if (!string.IsNullOrEmpty(addParam)) {
                //Récupération des paramètres optionnel avant nettoyage
                var addParamTypeTemp = addParam.Split(new[] { "|||" }, StringSplitOptions.None);
                if (addParamTypeTemp.Length > 1) {
                    model.IsForceReadOnly = GetAddParamValue(addParamTypeTemp[1], AlbParameterName.FORCEREADONLY) == "1";
                    model.IsIgnoreReadOnly = GetAddParamValue(addParamTypeTemp[1], AlbParameterName.IGNOREREADONLY) == "1";
                    model.IsAvnRefreshUserUpdate = GetAddParamValue(addParamTypeTemp[1], AlbParameterName.AVNREFRESHUSERUPDATE) == "1";
                    model.NumAvenantPage = GetAddParamValue(addParamTypeTemp[1], AlbParameterName.AVNID);
                    model.IsValidation = GetAddParamValue(addParamTypeTemp[1], AlbParameterName.VALIDATION) == "1";
                    model.NumAvenantExterne = GetAddParamValue(addParamTypeTemp[1], AlbParameterName.AVNIDEXTERNE);
                    model.ActeGestionRegule = GetAddParamValue(addParamTypeTemp[1], AlbParameterName.ACTEGESTIONREGULE);
                }

                //Nettoyage
                RemoveKeyFromAddParamValue(ref addParam, AlbParameterName.FORCEREADONLY);
                RemoveKeyFromAddParamValue(ref addParam, AlbParameterName.IGNOREREADONLY);
                RemoveKeyFromAddParamValue(ref addParam, AlbParameterName.AVNREFRESHUSERUPDATE);

                var addParamType = addParam.Split(new[] { "|||" }, StringSplitOptions.None);

                if (addParamType.Length > 1) {

                    model.AddParamType = addParamType[0];
                    var subTypeAddParam = addParamType[1].Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    model.AddParamValue = string.Join("||", subTypeAddParam);
                }
            }

            outId = outId.Contains("addParam") ? outId.Split(new[] { "addParam" }, StringSplitOptions.None)[0] : outId;

            #region Vérification des paramètres généraux
            string numeroParam = string.Empty;
            string versionParam = string.Empty;
            string typeParam = string.Empty;
            string redirect = string.Empty;
            if (!string.IsNullOrEmpty(outId)) {
                string[] tIds = outId.Split('_');
                if (tIds.Length > 2) {
                    numeroParam = tIds[0];
                    versionParam = tIds[1];
                    typeParam = tIds[2];

                }
            }

            #endregion

            model.CodePolicePage = numeroParam;
            model.VersionPolicePage = versionParam;
            model.TypePolicePage = typeParam;

            #region Type écran
            if (typeParam == AlbConstantesMetiers.TYPE_CONTRAT) {
                var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                switch (typeAvt) {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
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
            else {
                if (!string.IsNullOrEmpty(model.AddParamType))
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                else
                    model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
            }
            #endregion

            SetActeGestion(typeParam);

            if (string.IsNullOrWhiteSpace(model.NumAvenantPage)) {
                model.NumAvenantPage = default(int).ToString();
            }

            model.AllParameters = AlbParameters.Parse(outId);
        }

        protected void SetScreenType() {
            if (model.TypePolicePage == AlbConstantesMetiers.TYPE_CONTRAT) {
                //var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                switch (model.AllParameters.TypeAvenant) {
                    case AlbConstantesMetiers.TYPE_AVENANT_MODIF:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNMD;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_RESIL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_AVNRS;
                        break;
                    case AlbConstantesMetiers.TYPE_AVENANT_REGUL:
                        model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_REGUL;
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
            else {
                model.ScreenType = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
            }
        }

        protected void SetFolderId(string id, out string outId) {
            outId = id.Contains("tabGuid") ? id.Split(new[] { "tabGuid" }, StringSplitOptions.None)[0] : id;
        }

        protected Bandeau_MetaData GetInfoBandeau(string typeOffre) {
            var bandeau = new Bandeau_MetaData();
            if (typeOffre == AlbConstantesMetiers.TYPE_OFFRE) {
                GetInfoBandeauOffre(bandeau);
            }
            else if (typeOffre == AlbConstantesMetiers.TYPE_CONTRAT) {
                GetInfoBandeauContrat(bandeau);
            }
            return bandeau;
        }

        private void GetInfoBandeauOffre(Bandeau_MetaData bandeau) {
            if (this.model.Offre != null) {
                bandeau.IdOffre = model.Offre.CodeOffre;
                bandeau.Version = model.Offre.Version.HasValue ? model.Offre.Version.Value.ToString() : string.Empty;
                bandeau.Type = model.Offre.Type;
                bandeau.Description = model.Offre.Descriptif;
                bandeau.DateSaisie = model.Offre.DateSaisie.HasValue ? model.Offre.DateSaisie.Value.ToString("dd/MM/yyyy") : string.Empty;

                if (model.Offre.Branche != null) {
                    bandeau.Branche = model.Offre.Branche.Code;
                    bandeau.LibelleBranche = model.Offre.Branche.Nom;
                    if (model.Offre.Branche.Cible != null) {
                        bandeau.Cible = model.Offre.Branche.Cible.Code;
                        bandeau.LibelleCible = model.Offre.Branche.Cible.Nom;
                    }
                }
                //Vrai gestionnaire
                if (model.Offre.CabinetGestionnaire != null) {
                    bandeau.CodeCourtierGestionnaire = model.Offre.CabinetGestionnaire.Code.ToString(CultureInfo.CurrentCulture);
                    bandeau.NomCourtierGestionnaire = model.Offre.CabinetGestionnaire.NomCabinet;
                    if (model.Offre.CabinetGestionnaire.Adresse != null) {
                        bandeau.CPCourtier = model.Offre.CabinetGestionnaire.Adresse.CodePostal.ToString();
                        bandeau.VilleCourtierGestionnaire = model.Offre.CabinetGestionnaire.Adresse.NomVille;
                    }
                    if (model.Offre.CabinetGestionnaire.Delegation != null) {
                        bandeau.Delegation = model.Offre.CabinetGestionnaire.Delegation.Nom;
                        if (model.Offre.CabinetGestionnaire.Inspecteur != null) {
                            bandeau.Inspecteur = model.Offre.CabinetGestionnaire.Inspecteur;
                        }
                        bandeau.Secteur = model.Offre.CabinetGestionnaire.Delegation.Secteur;
                    }
                }
                //Vrai apporteur
                if (model.Offre.CabinetApporteur != null) {
                    bandeau.IdCourtier = model.Offre.CabinetApporteur.Code.ToString(CultureInfo.CurrentCulture);
                    bandeau.NomCourtier = model.Offre.CabinetApporteur.NomCabinet;
                    if (model.Offre.CabinetApporteur.Adresse != null) {
                        bandeau.VilleCourtier = model.Offre.CabinetApporteur.Adresse.NomVille;
                    }

                }
                if (model.Offre.PreneurAssurance != null) {
                    bandeau.IdAssure = model.Offre.PreneurAssurance.Code;
                    bandeau.NomAssure = model.Offre.PreneurAssurance.NomAssure;
                    if (model.Offre.PreneurAssurance.Adresse != null) {
                        bandeau.CPAssure = model.Offre.PreneurAssurance.Adresse.CodePostal.ToString();
                        bandeau.VilleAssure = model.Offre.PreneurAssurance.Adresse.NomVille;
                    }
                }
                if (model.Offre.DateEffetGarantie.HasValue) {
                    bandeau.DateDebEffet = string.Format("{0}/{1}/{2}", model.Offre.DateEffetGarantie.Value.Day.ToString(CultureInfo.CurrentCulture).PadLeft(2, '0'), model.Offre.DateEffetGarantie.Value.Month.ToString().PadLeft(2, '0'), model.Offre.DateEffetGarantie.Value.Year);
                }
                if (model.Offre.DateFinEffetGarantie.HasValue) {
                    bandeau.DateFinEffet = string.Format("{0}/{1}/{2}", model.Offre.DateFinEffetGarantie.Value.Day.ToString(CultureInfo.CurrentCulture).PadLeft(2, '0'), model.Offre.DateFinEffetGarantie.Value.Month.ToString().PadLeft(2, '0'), model.Offre.DateFinEffetGarantie.Value.Year);
                }

                bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                bandeau.HasDoubleSaisie = model.Offre.HasDoubleSaisie;
                bandeau.CodeDevise = model.Offre.Devise != null ? model.Offre.Devise.Code : string.Empty;
                bandeau.LibelleDevise = model.Offre.Devise != null ? model.Offre.Devise.Libelle : string.Empty;
                bandeau.NatureContrat = model.Offre.NatureContrat != null ? model.Offre.NatureContrat.Code : string.Empty;
                bandeau.LibelleNatureContrat = model.Offre.NatureContrat != null ? model.Offre.NatureContrat.Libelle : string.Empty;
                bandeau.Part = model.Offre.PartAlbingia.HasValue ? model.Offre.PartAlbingia.Value + "%" : string.Empty;
                bandeau.Couverture = model.Offre.Couverture.HasValue ? model.Offre.Couverture.Value + "%" : string.Empty;
                bandeau.GestionnaireCode = model.Offre.GestionnaireCode;
                bandeau.GestionnaireNom = model.Offre.GestionnaireNom;
                bandeau.SouscripteurCode = model.Offre.SouscripteurCode;
                bandeau.SouscripteurNom = model.Offre.SouscripteurNom;
                bandeau.LibelleIndice = model.Offre.IndiceReference != null ? model.Offre.IndiceReference.Libelle : string.Empty;
                bandeau.CodeIndice = model.Offre.IndiceReference != null ? model.Offre.IndiceReference.Code : string.Empty;
                bandeau.Valeur = model.Offre.Valeur;
                bandeau.CodeEtat = model.Offre.Etat;
                bandeau.LibelleEtat = model.Offre.LibelleEtat;
                bandeau.CodeSituation = model.Offre.Situation;
            }
        }

        private void GetInfoBandeauContrat(Bandeau_MetaData bandeau) {
            var contrat = this.model.Contrat;
            if (contrat != null) {
                bandeau.IdOffre = contrat.CodeContrat;
                bandeau.Version = contrat.VersionContrat.ToString();
                bandeau.Type = contrat.Type;
                bandeau.Cible = contrat.Cible;
                bandeau.LibelleCible = contrat.CibleLib;
                bandeau.Description = contrat.Descriptif;
                bandeau.Branche = contrat.Branche;
                bandeau.LibelleBranche = contrat.BrancheLib;
                bandeau.GestionnaireCode = contrat.GestionnaireCode;
                bandeau.GestionnaireNom = contrat.GestionnaireNom;
                bandeau.NumAvenant = int.TryParse(model.NumAvenantPage, out var numAvt) ? numAvt : 0;

                if (contrat.DateEffetAnnee != 0 && contrat.DateEffetMois != 0 && contrat.DateEffetJour != 0) {
                    bandeau.DateDebEffet = new DateTime(contrat.DateEffetAnnee, contrat.DateEffetMois, contrat.DateEffetJour).ToShortDateString();
                }

                if (contrat.FinEffetAnnee != 0 && contrat.FinEffetMois != 0 && contrat.FinEffetJour != 0) {
                    bandeau.DateFinEffet = new DateTime(contrat.FinEffetAnnee, contrat.FinEffetMois, contrat.FinEffetJour).ToShortDateString();
                }
                bandeau.Periodicite = contrat.PeriodiciteNom;
                bandeau.IdAssure = contrat.CodePreneurAssurance.ToString();
                bandeau.NomAssure = contrat.NomPreneurAssurance;
                bandeau.VilleAssure = contrat.VilleAssure;
                bandeau.CPAssure = contrat.DepAssure + contrat.CodePostalAssure;
                bandeau.NatureContrat = contrat.NatureContrat;
                bandeau.LibelleNatureContrat = contrat.LibelleNatureContrat;
                bandeau.CodeEtat = contrat.Etat;
                bandeau.LibelleEtat = contrat.LibelleEtat;
                bandeau.CodeSituation = contrat.Situation;
                bandeau.LibelleSituation = contrat.LibelleSituation;
                bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                bandeau.Part = contrat.PartAlbingia.HasValue ? contrat.PartAlbingia.Value + "%" : string.Empty;
                bandeau.Couverture = contrat.Couverture + "%";
                bandeau.CodeRegime = contrat.CodeRegime;
                bandeau.SoumisCatNat = !string.IsNullOrEmpty(contrat.SoumisCatNat) ? contrat.SoumisCatNat == "O" ? "oui" : "non" : string.Empty;
                bandeau.CodeEncaissement = contrat.CodeEncaissement;
                bandeau.LibelleEncaissement = contrat.LibelleEncaissement;
                bandeau.EcheancePrincipale = contrat.Jour + "/" + contrat.Mois;
                bandeau.CodeIndice = contrat.IndiceReference;
                bandeau.LibelleIndice = contrat.LibelleIndicReference;
                bandeau.Valeur = Convert.ToDecimal(contrat.Valeur);
                bandeau.CodeDevise = contrat.Devise;
                bandeau.LibelleDevise = contrat.LibelleDevise;
                bandeau.Inspecteur = contrat?.Inspecteur;
                bandeau.Delegation = contrat?.Delegation;
            }
        }

        protected Bandeau_MetaData GetInfoBandeauRechercheSaisie(string Id, bool modeHisto = false) {
            var bandeau = new Bandeau_MetaData();
            string[] tId = Id.Split('_');
            using (var channelClient = ServiceClientFactory.GetClient<ICommonOffre>()) {
                var commonOffreClient = channelClient.Channel;
                BandeauDto bandeauDto = null;
                if (modeHisto) {
                    bandeauDto = commonOffreClient.GetBandeauHisto(tId[0], tId[1], tId[2], tId[3]);
                }
                else {
                    bandeauDto = commonOffreClient.GetBandeau(tId[0], tId[1], tId[2]);
                }
                if (bandeauDto is null) {
                    return bandeau;
                }

                if (tId[2] == AlbConstantesMetiers.TYPE_OFFRE) {
                    bandeau.IdOffre = bandeauDto.CodeOffre;
                    bandeau.Version = bandeauDto.VersionOffre.ToString(CultureInfo.InvariantCulture);
                    bandeau.Type = bandeau.Type;
                    bandeau.Description = bandeauDto.Descriptif;
                    if (bandeauDto.DateSaisieAnnee != 0 && bandeauDto.DateSaisieMois != 0 && bandeauDto.DateSaisieJour != 0)
                        bandeau.DateSaisie = new DateTime(bandeauDto.DateSaisieAnnee, bandeauDto.DateSaisieMois, bandeauDto.DateSaisieJour).ToString("dd/MM/yyyy");
                    else
                        bandeau.DateSaisie = string.Empty;
                    bandeau.Branche = bandeauDto.BrancheCode;
                    bandeau.LibelleBranche = bandeauDto.BrancheLib;
                    bandeau.Cible = bandeauDto.CibleCode;
                    bandeau.LibelleCible = bandeauDto.CibleLib;
                    bandeau.Periodicite = bandeauDto.PeriodiciteNom;
                    bandeau.EcheancePrincipale = bandeauDto.EchJour + "/" + bandeauDto.EchMois;
                    if (bandeauDto.ProchaineEchAnnee != 0 && bandeauDto.ProchaineEchMois != 0 && bandeauDto.ProchaineEchJour != 0)
                        bandeau.ProchaineEcheance = new DateTime(bandeauDto.ProchaineEchAnnee, bandeauDto.ProchaineEchMois, bandeauDto.ProchaineEchJour);
                    if (bandeauDto.CodeCourtierGestionnaire != 0) {
                        bandeau.CodeCourtierGestionnaire = bandeauDto.CodeCourtierGestionnaire.ToString(CultureInfo.CurrentCulture);
                        bandeau.NomCourtierGestionnaire = bandeauDto.NomCourtierGest;
                        bandeau.CPCourtier = bandeauDto.CPCourtierGest;
                        bandeau.VilleCourtierGestionnaire = bandeauDto.VilleCourtierGest;
                        bandeau.Delegation = bandeauDto.NomDelegation;
                        bandeau.Inspecteur = bandeauDto.NomInspecteur;
                        bandeau.Secteur = bandeauDto.Secteur;
                        bandeauDto.LibSecteur = bandeauDto.LibSecteur;
                    }
                    if (bandeauDto.CodeCourtierApporteur != 0) {
                        bandeau.IdCourtier = bandeauDto.CodeCourtierApporteur.ToString(CultureInfo.CurrentCulture);
                        bandeau.NomCourtier = bandeauDto.NomCourtierAppo;
                        bandeau.VilleCourtier = bandeauDto.VilleCourtierAppo;
                    }
                    using (var channelClientPol = ServiceClientFactory.GetClient<IPoliceServices>()) {
                        var policeServices = channelClientPol.Channel;
                        DelegationDto delegation = policeServices.ObtenirDelegation(bandeauDto.CodeCourtierGestionnaire);
                        if (delegation != null) {
                            bandeau.Delegation = delegation.Nom;
                            bandeau.Inspecteur = delegation.Inspecteur != null ? delegation.Inspecteur.Nom : string.Empty;
                            bandeau.Secteur = delegation.Secteur;
                            bandeau.LibSecteur = delegation.LibSecteur;
                        }
                    }
                    if (bandeauDto.CodePreneurAssurance != 0) {
                        bandeau.IdAssure = bandeauDto.CodePreneurAssurance.ToString(CultureInfo.InvariantCulture);
                        bandeau.NomAssure = bandeauDto.NomPreneurAssurance;
                        bandeau.CPAssure = bandeauDto.CPPreneurAssurance;
                        bandeau.VilleAssure = bandeauDto.VillePreneurAssurance;
                    }

                    if (bandeauDto.DateEffetAnnee != 0 && bandeauDto.DateEffetMois != 0 && bandeauDto.DateEffetJour != 0)
                        bandeau.DateDebEffet = new DateTime(bandeauDto.DateEffetAnnee, bandeauDto.DateEffetMois, bandeauDto.DateEffetJour).ToShortDateString();

                    if (bandeauDto.FinEffetAnnee != 0 && bandeauDto.FinEffetMois != 0 && bandeauDto.FinEffetJour != 0)
                        bandeau.DateFinEffet = new DateTime(bandeauDto.FinEffetAnnee, bandeauDto.FinEffetMois, bandeauDto.FinEffetJour).ToShortDateString();

                    switch (bandeauDto.TypeOffre) {
                        case AlbConstantesMetiers.TYPE_OFFRE:
                            bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                            break;
                        case AlbConstantesMetiers.TYPE_CONTRAT:
                            bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                            break;
                        default:
                            bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_OFFRE;
                            break;
                    }
                    bandeau.HasDoubleSaisie = bandeauDto.HasDoubleSaisie;
                    bandeau.CodeDevise = bandeauDto.CodeDevise;
                    bandeau.LibelleDevise = bandeauDto.LibelleDevise;
                    bandeau.NatureContrat = bandeauDto.NatureContrat;
                    bandeau.LibelleNatureContrat = bandeauDto.LibelleNatureContrat;
                    bandeau.Part = bandeauDto.PartAlbingia.HasValue ? bandeauDto.PartAlbingia.Value + "%" : string.Empty;
                    bandeau.Couverture = bandeauDto.Couverture + "%";
                    bandeau.LibelleIndice = bandeauDto.CodeIndiceReference = bandeauDto.LibelleIndicReference;
                    bandeau.CodeIndice = bandeauDto.CodeIndiceReference;
                    bandeau.Valeur = Convert.ToDecimal(bandeauDto.Valeur.ToString());
                    bandeau.CodeEtat = bandeauDto.Etat;
                    bandeau.LibelleEtat = bandeauDto.LibelleEtat;
                    bandeau.CodeSituation = bandeauDto.CodeSituation;
                    bandeau.CodeRegime = bandeauDto.CodeRegime;
                    bandeau.LibelleRegime = bandeauDto.LibelleRegime;
                    bandeau.SoumisCatNat = !string.IsNullOrEmpty(bandeauDto.SoumisCatNat) ? bandeauDto.SoumisCatNat == "O" ? "oui" : "non" : string.Empty;
                    bandeau.MontantReference = (decimal)(bandeauDto.MontantRef1 != 0 ? bandeauDto.MontantRef1 : bandeauDto.MontantRef2);
                    bandeau.Indexation = bandeauDto.Indexation == "O" ? "oui" : "non";
                    bandeau.LCI = bandeauDto.LCI == "O" ? "oui" : "non";
                    bandeau.Assiette = bandeauDto.Assiette == "O" ? "oui" : "non";
                    bandeau.Franchise = bandeauDto.Franchise == "O" ? "oui" : "non";
                    bandeau.CodeAction = bandeauDto.CodeAction;
                    bandeau.LibelleAction = bandeauDto.LibelleAction;
                    if (bandeauDto.DateSituationAnnee != 0 && bandeauDto.DateSituationMois != 0 && bandeauDto.DateSituationJour != 0)
                        bandeau.DateSituation = new DateTime(bandeauDto.DateSituationAnnee, bandeauDto.DateSituationMois, bandeauDto.DateSituationJour);
                    bandeau.CodeUsrCreateur = bandeauDto.CodeUsrCreateur;
                    bandeau.NomUsrCreateur = bandeauDto.NomUsrCreateur;
                    bandeau.CodeUsrModificateur = bandeauDto.CodeUsrModificateur;
                    bandeau.NomUsrModificateur = bandeauDto.NomUsrModificateur;
                    if (bandeauDto.DateEnregistrementAnnee != 0 && bandeauDto.DateEnregistrementMois != 0 && bandeauDto.DateEnregistrementJour != 0)
                        bandeau.DateEnregistrement = new DateTime(bandeauDto.DateEnregistrementAnnee, bandeauDto.DateEnregistrementMois, bandeauDto.DateEnregistrementJour);
                    if (bandeauDto.DateMAJAnnee != 0 && bandeauDto.DateMAJMois != 0 && bandeauDto.DateMAJJour != 0)
                        bandeau.DateMAJ = new DateTime(bandeauDto.DateMAJAnnee, bandeauDto.DateMAJMois, bandeauDto.DateMAJJour);
                    bandeau.Preavis = bandeauDto.Preavis;
                    bandeau.LibelleSituation = bandeauDto.LibelleSituation;
                    bandeau.HorsCatNat = bandeauDto.HorsCatNat;
                    bandeau.CatNat = bandeauDto.CatNat;
                    bandeau.TauxHorsCatNat = bandeauDto.TauxHorsCatNat;
                    bandeau.TauxCatNat = bandeauDto.TauxCatNat;
                }
                else {
                    using (var channelClientPol = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IPoliceServices>()) {
                        var policeServices = channelClientPol.Channel;
                        DelegationDto delegation = policeServices.ObtenirDelegation(bandeauDto.CodeCourtierApporteur);
                        if (delegation != null) {
                            bandeau.Delegation = delegation.Nom;
                            bandeau.Inspecteur = delegation.Inspecteur != null ? delegation.Inspecteur.Nom : string.Empty;
                            bandeau.Secteur = delegation.Secteur;
                            bandeau.LibSecteur = delegation.LibSecteur;
                        }
                    }

                    bandeau.IdOffre = bandeauDto.CodeOffre;
                    bandeau.Version = bandeauDto.VersionOffre.ToString(CultureInfo.InvariantCulture);
                    bandeau.Type = bandeauDto.TypeOffre;
                    bandeau.Cible = bandeauDto.CibleCode;
                    bandeau.LibelleCible = bandeauDto.CibleLib;
                    bandeau.Description = bandeauDto.Descriptif;
                    bandeau.Branche = bandeauDto.BrancheCode;
                    bandeau.LibelleBranche = bandeauDto.BrancheLib;
                    bandeau.GestionnaireCode = bandeauDto.GestionnaireCode;
                    bandeau.GestionnaireNom = bandeauDto.GestionnaireNom;

                    if (bandeauDto.DateEffetAnnee != 0 && bandeauDto.DateEffetMois != 0 && bandeauDto.DateEffetJour != 0) {
                        bandeau.DateDebEffet = new DateTime(bandeauDto.DateEffetAnnee, bandeauDto.DateEffetMois, bandeauDto.DateEffetJour).ToShortDateString();
                    }

                    if (bandeauDto.FinEffetAnnee != 0 && bandeauDto.FinEffetMois != 0 && bandeauDto.FinEffetJour != 0) {
                        bandeau.DateFinEffet = new DateTime(bandeauDto.FinEffetAnnee, bandeauDto.FinEffetMois, bandeauDto.FinEffetJour).ToShortDateString();
                    }
                    bandeau.Periodicite = bandeauDto.PeriodiciteNom;
                    bandeau.IdAssure = bandeauDto.CodePreneurAssurance.ToString();
                    bandeau.NomAssure = bandeauDto.NomPreneurAssurance;
                    bandeau.CPAssure = bandeauDto.CPPreneurAssurance;
                    bandeau.VilleAssure = bandeauDto.VillePreneurAssurance;
                    bandeau.NatureContrat = bandeauDto.NatureContrat;
                    bandeau.LibelleNatureContrat = bandeauDto.LibelleNatureContrat;
                    bandeau.CodeEtat = bandeauDto.Etat;
                    bandeau.LibelleEtat = bandeauDto.LibelleEtat;
                    bandeau.CodeSituation = bandeauDto.CodeSituation;
                    bandeau.LibelleSituation = bandeauDto.LibelleSituation;

                    if (bandeauDto.CodeCourtierApporteur != 0) {
                        bandeau.IdCourtier = bandeauDto.CodeCourtierApporteur.ToString();
                        bandeau.NomCourtier = bandeauDto.NomCourtierAppo;
                        bandeau.VilleCourtier = !string.IsNullOrEmpty(bandeauDto.VilleCourtierAppo) && bandeauDto.VilleCourtierAppo.Trim() != "0" ? bandeauDto.VilleCourtierAppo : string.Empty;
                    }

                    if (bandeauDto.CodeCourtierGestionnaire != 0) {
                        bandeau.CodeCourtierGestionnaire = bandeauDto.CodeCourtierGestionnaire.ToString();
                        bandeau.NomCourtierGestionnaire = bandeauDto.NomCourtierGest;
                        bandeau.VilleCourtierGestionnaire = bandeauDto.CPCourtierGest + " " + bandeauDto.VilleCourtierGest;
                    }

                    if (bandeauDto.CodeCourtierPayeur != 0) {
                        bandeau.CodeCourtierPayeur = bandeauDto.CodeCourtierPayeur.ToString();
                        bandeau.NomCourtierPayeur = bandeauDto.NomCourtierPayeur;
                        bandeau.VilleCourtierPayeur = !string.IsNullOrEmpty(bandeauDto.VilleCourtierPayeur) && bandeauDto.VilleCourtierPayeur.Trim() != "0" ? bandeauDto.VilleCourtierPayeur : string.Empty;
                    }

                    bandeau.StyleBandeau = AlbConstantesMetiers.SCREEN_TYPE_CONTRAT;
                    bandeau.Part = bandeauDto.PartAlbingia.HasValue ? bandeauDto.PartAlbingia.Value + "%" : string.Empty;
                    bandeau.Couverture = bandeauDto.Couverture + "%";
                    bandeau.CodeRegime = bandeauDto.CodeRegime;
                    bandeau.SoumisCatNat = !string.IsNullOrEmpty(bandeauDto.SoumisCatNat) ? bandeauDto.SoumisCatNat == "O" ? "oui" : "non" : string.Empty;
                    bandeau.CodeEncaissement = bandeauDto.CodeEncaissement;
                    bandeau.LibelleEncaissement = bandeauDto.LibelleEncaissement;
                    bandeau.EcheancePrincipale = bandeauDto.EchJour + "/" + bandeauDto.EchMois;
                    bandeau.CodeIndice = bandeauDto.CodeIndiceReference;
                    bandeau.LibelleIndice = bandeauDto.LibelleIndicReference;
                    bandeau.Valeur = Convert.ToDecimal(bandeauDto.Valeur);
                    bandeau.CodeDevise = bandeauDto.CodeDevise;
                    bandeau.LibelleDevise = bandeauDto.LibelleDevise;
                    using (var channelClientAff = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffaireNouvelle>()) {
                        var serviceContext = channelClientAff.Channel;
                        bandeau.MontantStatistique = serviceContext.GetMontantStatistique(tId[0], tId[1]);
                    }

                    bandeau.CodeAction = bandeauDto.CodeAction;
                    bandeau.LibelleAction = bandeauDto.LibelleAction;
                    bandeau.LibelleRegime = bandeauDto.LibelleRegime;
                    bandeau.MontantReference = (decimal)(bandeauDto.MontantRef1 != 0 ? bandeauDto.MontantRef1 : bandeauDto.MontantRef2);
                    if (bandeauDto.DateEffetAvenantAnnee != 0 && bandeauDto.DateEffetAvenantMois != 0 && bandeauDto.DateEffetAvenantJour != 0)
                        bandeau.DateEffetAvenant = new DateTime(bandeauDto.DateEffetAvenantAnnee, bandeauDto.DateEffetAvenantMois, bandeauDto.DateEffetAvenantJour);
                    bandeau.CodeOffreOrigine = bandeauDto.CodeOffreOrigine;
                    bandeau.VersionOffreOrigine = bandeauDto.VersionOffreOrigine;
                    bandeau.NumAvenant = bandeauDto.NumAvenant;
                    bandeau.NumExterne = bandeauDto.NumExterne;
                    bandeau.Indexation = bandeauDto.Indexation == "O" ? "oui" : "non";
                    bandeau.LCI = bandeauDto.LCI == "O" ? "oui" : "non";
                    bandeau.Assiette = bandeauDto.Assiette == "O" ? "oui" : "non";
                    bandeau.Franchise = bandeauDto.Franchise == "O" ? "oui" : "non";
                    if (bandeauDto.ProchaineEchAnnee != 0 && bandeauDto.ProchaineEchMois != 0 && bandeauDto.ProchaineEchJour != 0)
                        bandeau.ProchaineEcheance = new DateTime(bandeauDto.ProchaineEchAnnee, bandeauDto.ProchaineEchMois, bandeauDto.ProchaineEchJour);
                    bandeau.HorsCatNat = bandeauDto.HorsCatNat;
                    bandeau.CatNat = bandeauDto.CatNat;
                    bandeau.TauxHorsCatNat = bandeauDto.TauxHorsCatNat;
                    bandeau.TauxCatNat = bandeauDto.TauxCatNat;
                    if (bandeauDto.DateaffaireNouvelleAnnee != 0 && bandeauDto.DateaffaireNouvelleMois != 0 && bandeauDto.DateaffaireNouvelleJour != 0)
                        bandeau.DateaffaireNouvelle = new DateTime(bandeauDto.DateaffaireNouvelleAnnee, bandeauDto.DateaffaireNouvelleMois, bandeauDto.DateaffaireNouvelleJour);
                    if (bandeauDto.DateSituationAnnee != 0 && bandeauDto.DateSituationMois != 0 && bandeauDto.DateSituationJour != 0)
                        bandeau.DateSituation = new DateTime(bandeauDto.DateSituationAnnee, bandeauDto.DateSituationMois, bandeauDto.DateSituationJour);
                    bandeau.CodeUsrCreateur = bandeauDto.CodeUsrCreateur;
                    bandeau.NomUsrCreateur = bandeauDto.NomUsrCreateur;
                    bandeau.CodeUsrModificateur = bandeauDto.CodeUsrModificateur;
                    bandeau.NomUsrModificateur = bandeauDto.NomUsrModificateur;
                    if (bandeauDto.DateEnregistrementAnnee != 0 && bandeauDto.DateEnregistrementMois != 0 && bandeauDto.DateEnregistrementJour != 0)
                        bandeau.DateEnregistrement = new DateTime(bandeauDto.DateEnregistrementAnnee, bandeauDto.DateEnregistrementMois, bandeauDto.DateEnregistrementJour);
                    if (bandeauDto.DateMAJAnnee != 0 && bandeauDto.DateMAJMois != 0 && bandeauDto.DateMAJJour != 0)
                        bandeau.DateMAJ = new DateTime(bandeauDto.DateMAJAnnee, bandeauDto.DateMAJMois, bandeauDto.DateMAJJour);
                    bandeau.Preavis = bandeauDto.Preavis;
                    bandeau.Stop = bandeauDto.Stop;
                    bandeau.StopLib = bandeauDto.StopLib;
                    bandeau.StopContentieux = bandeauDto.StopContentieux;

                }
                bandeau.LCIGenerale = bandeauDto.LCIGenerale;
                bandeau.LCIGeneraleType = bandeauDto.LCIGeneraleType;
                bandeau.LCIGeneraleUnit = bandeauDto.LCIGeneraleUnit;
                bandeau.FranchiseGenerale = bandeauDto.FranchiseGenerale;
                bandeau.FranchiseGeneraleType = bandeauDto.FranchiseGeneraleType;
                bandeau.FranchiseGeneraleUnit = bandeauDto.FranchiseGeneraleUnit;
                bandeau.SouscripteurPrenom = bandeauDto.SouscripteurPrenom;
                bandeau.GestionnairePrenom = bandeauDto.GestionnairePrenom;
                bandeau.GestionnaireCode = bandeauDto.GestionnaireCode;
                bandeau.GestionnaireNom = bandeauDto.GestionnaireNom;
                bandeau.SouscripteurCode = bandeauDto.SouscripteurCode;
                bandeau.SouscripteurNom = bandeauDto.SouscripteurNom;
                bandeau.Territorialite = bandeauDto.Territorialite;
                bandeau.TerritorialiteLib = bandeauDto.TerritorialiteLib;
                bandeau.CodeMotif = bandeauDto.CodeMotif;
                bandeau.LibelleMotif = bandeauDto.LibelleMotif;
                bandeau.Duree = bandeauDto.Duree;
                bandeau.DureeUnite = bandeauDto.DureeUnite;
                bandeau.DureeStr = bandeauDto.DureeStr;
                bandeau.HasSusp = bandeauDto.HasSusp;
                bandeau.Origine = bandeauDto.Origine;
                bandeau.TauxAvailable = bandeauDto.TauxAvailable;
                return bandeau;
            }
        }

        protected bool DisplayBandeau(bool isDisplayed, string numOffre) {
            bool toReturn = true;
            using (var channelClient = ServiceClientFactory.GetClient<IPoliceServices>()) {
                if (!isDisplayed || string.IsNullOrEmpty(numOffre) || !numOffre.Contains("_") || !channelClient.Channel.TestExistanceOffre(numOffre.Split('_')[0])) {
                    toReturn = false;
                }
            }
            return toReturn;
        }

        protected bool GetAdminUser() {
            return CacheUserRights.IsUserAdmin;
        }

        protected void DisplayBandeau() {
            model.AfficherBandeau = true;
        }

        protected void SetActeGestion(string type) {
            switch (type) {
                case "O":
                    model.ActeGestion = "OFFRE";
                    break;
                case "P":
                    var typeAvt = GetAddParamValue(model.AddParamValue, AlbParameterName.AVNTYPE);
                    model.ActeGestion = string.IsNullOrEmpty(typeAvt) ? "AFFNOUV" : typeAvt;
                    break;
                default:
                    model.ActeGestion = "";
                    break;
            }
        }

        /// <summary>
        /// Initializes session lock before effective DB lock
        /// </summary>
        /// <param name="ipb"></param>
        /// <param name="alx"></param>
        protected void InitVerrouillage(string ipb, int alx = 0) {
            if (IsReadonly) {
                return;
            }
            AccesAffaire acces = null;
            switch (Model) {
                case ModeleCreationSaisiePage createOffre:
                case AnCreationContratPage createContrat:
                case ModeleCreationAffaireNouvellePage createAN:
                    acces = new AccesAffaire(AccesOrigine.Modifier) {
                        Code = ipb.ToIPB(),
                        Version = alx < 0 ? 0 : alx,
                        Avenant = null,
                        TabGuid = Guid.Parse(this.model.TabGuid)
                    };
                    break;
                default:
                    break;
            }
            if (acces != null) {
                MvcApplication.ListeAccesAffaires.Add(acces);
            }
        }

        protected string GetAddParamValue(string addParamValue, AlbParameterName nameVariable) {
            return InformationGeneraleTransverse.GetAddParamValue(addParamValue, nameVariable);
        }

        protected void SetAddParamValue(ref string addParamValue, AlbParameterName nameVariable, string value) {
            InformationGeneraleTransverse.SetAddParamValue(ref addParamValue, nameVariable, value);
        }

        protected void RemoveKeyFromAddParamValue(ref string addParamValue, AlbParameterName nameVariable) {
            InformationGeneraleTransverse.RemoveKeyFromAddParamValue(ref addParamValue, nameVariable);
        }
        #region Vérification Partenaires / LABLAT
        /// <summary>
        /// Utilisation du service LABLAT
        /// </summary>
        /// <returns></returns>
        protected bool CanUseLablat() {
            return ConfigurationManager.AppSettings["useLablat"].AsBoolean().GetValueOrDefault();
        }

        /// <summary>
        /// Vérification partenaires / LABLAT
        /// </summary>
        /// <param name="partenaires">Partenaires</param>
        protected void VerificationPartenaires(PartenairesDto partenaires) {
            if (!CanUseLablat()) { return; }
            var codeCourtierGestionnaire = partenaires.CourtierGestionnaire?.Code.ParseInt().Value;
            var codeCourtierApporteur = partenaires.CourtierApporteur?.Code.ParseInt().Value;
            var codeCourtierPayeur = partenaires.CourtierPayeur?.Code.ParseInt().Value;
            var partners = new List<LablatPartner>();

            // Courtier gestionnaire
            if (codeCourtierGestionnaire != 0 && (partenaires.CourtierGestionnaire?.Nom).ContainsChars()) {
                partners.Add(new LablatPartner {
                    Code = partenaires.CourtierGestionnaire.Code,
                    Name = partenaires.CourtierGestionnaire.Nom,
                    Type = TypePartenaireKheops.CourtierGestionnaire,
                    TypeAS400 = TypePartenaireAs400.Courtier
                });
                // Interlocuteur
                if (partenaires.CourtierGestionnaire?.CodeInterlocuteur != 0 && !string.IsNullOrEmpty(partenaires.CourtierGestionnaire?.NomInterlocuteur)) {
                    partners.Add(new LablatPartner {
                        Code = partenaires.CourtierGestionnaire.Code,
                        Name = partenaires.CourtierGestionnaire.NomInterlocuteur,
                        RepresentativeCode = partenaires.CourtierGestionnaire.CodeInterlocuteur?.ToString(),
                        Type = TypePartenaireKheops.Interlocuteur,
                        TypeAS400 = TypePartenaireAs400.Courtier
                    });
                }
            }
            if (codeCourtierGestionnaire != codeCourtierApporteur) {
                // Courtier apporteur
                if (codeCourtierApporteur != 0 && !string.IsNullOrEmpty(partenaires.CourtierApporteur?.Nom)) {
                    partners.Add(new LablatPartner {
                        Code = partenaires.CourtierApporteur.Code,
                        Name = partenaires.CourtierApporteur.Nom,
                        Type = TypePartenaireKheops.CourtierApporteur,
                        TypeAS400 = TypePartenaireAs400.Courtier
                    });
                }
            }
            if (codeCourtierGestionnaire != codeCourtierPayeur && codeCourtierApporteur != codeCourtierPayeur) {
                // Courtier payeur
                if (codeCourtierPayeur != 0 && !string.IsNullOrEmpty(partenaires.CourtierPayeur?.Nom)) {
                    partners.Add(new LablatPartner {
                        Code = partenaires.CourtierPayeur.Code,
                        Name = partenaires.CourtierPayeur.Nom,
                        Type = TypePartenaireKheops.CourtierPayeur,
                        TypeAS400 = TypePartenaireAs400.Courtier
                    });
                }
            }
            // Preneur d'assurance
            if (partenaires.PreneurAssurance?.Code.ParseInt() != 0 && !string.IsNullOrEmpty(partenaires.PreneurAssurance?.Nom)) {
                partners.Add(new LablatPartner {
                    Code = partenaires.PreneurAssurance.Code,
                    Name = partenaires.PreneurAssurance.Nom,
                    Type = TypePartenaireKheops.PreneurAssurance,
                    TypeAS400 = TypePartenaireAs400.Assure
                });
            }
            // assurés additionnels
            if (partenaires.AssuresAdditionnels?.Count > 0) {
                foreach (var assuresAdditionnel in partenaires.AssuresAdditionnels) {
                    if (!string.IsNullOrEmpty(assuresAdditionnel.Code) && !string.IsNullOrEmpty(assuresAdditionnel.Nom)) {
                        partners.Add(new LablatPartner {
                            Code = assuresAdditionnel.Code,
                            Name = assuresAdditionnel.Nom,
                            Type = TypePartenaireKheops.AssureAdditionnel,
                            TypeAS400 = TypePartenaireAs400.Assure
                        });
                    }
                }
            }
            // Coassureurs
            if (partenaires.Coassureurs?.Count > 0) {
                foreach (var coassureur in partenaires.Coassureurs) {
                    if (coassureur.CodeInterlocuteur == 0) {
                        if (!string.IsNullOrEmpty(coassureur.Code) && !string.IsNullOrEmpty(coassureur.Nom)) {
                            partners.Add(new LablatPartner {
                                Code = coassureur.Code,
                                Name = coassureur.Nom,
                                Type = TypePartenaireKheops.CoAssureur,
                                TypeAS400 = TypePartenaireAs400.Compagnie
                            });
                        }
                    }
                    else {
                        // Interlocuteur co-assureur
                        if (!string.IsNullOrEmpty(coassureur.Code) && !string.IsNullOrEmpty(coassureur.NomInterlocuteur)) {
                            partners.Add(new LablatPartner {
                                Code = coassureur.Code,
                                Name = coassureur.NomInterlocuteur,
                                RepresentativeCode = coassureur.CodeInterlocuteur?.ToString(),
                                Type = TypePartenaireKheops.InterlocuteurCoAssureur,
                                TypeAS400 = TypePartenaireAs400.Compagnie
                            });
                        }
                    }

                }
            }
            // Intervenants
            if (partenaires.Intervenants?.Count > 0) {
                foreach (var intervenant in partenaires.Intervenants) {
                    if (intervenant.CodeInterlocuteur == 0) {
                        // Intervenants
                        if (!string.IsNullOrEmpty(intervenant.Code) && !string.IsNullOrEmpty(intervenant.Nom)) {
                            partners.Add(new LablatPartner {
                                Code = intervenant.Code,
                                Name = intervenant.Nom,
                                Type = TypePartenaireKheops.Intervenant,
                                TypeAS400 = TypePartenaireAs400.Intervenant
                            });
                        }
                    }
                    else {
                        // Interlocuteur intervenant
                        if (!string.IsNullOrEmpty(intervenant.Code) && !string.IsNullOrEmpty(intervenant.NomInterlocuteur)) {
                            partners.Add(new LablatPartner {
                                Code = intervenant.Code,
                                Name = intervenant.NomInterlocuteur,
                                RepresentativeCode = intervenant.CodeInterlocuteur?.ToString(),
                                Type = TypePartenaireKheops.InterlocuteurIntervenant,
                                TypeAS400 = TypePartenaireAs400.Compagnie
                            });
                        }
                    }

                }
            }
            // courtiers additionnels
            if (partenaires.CourtiersAdditionnels?.Count > 0) {
                foreach (var courtierAdditionnel in partenaires.CourtiersAdditionnels) {
                    if (!string.IsNullOrEmpty(courtierAdditionnel.Code) && !string.IsNullOrEmpty(courtierAdditionnel.Nom)) {
                        partners.Add(new LablatPartner {
                            Code = courtierAdditionnel.Code,
                            Name = courtierAdditionnel.Nom,
                            Type = TypePartenaireKheops.CourtierAdditionnel,
                            TypeAS400 = TypePartenaireAs400.Courtier
                        });
                    }
                }
            }

            CheckPartners(partners.Select(x => x.ToPartner()));
        }

        protected BlacklistAlert CheckPartner(LablatPartner partner) {
            var result = LablatResult.Error;
            using (var client = ServiceClientFactory.GetClient<ILablat>()) {
                result = client.Channel.VerificationPartenaire(
                    Convert.ToInt32(AlbEnumInfoValue.GetEnumInfo(partner.TypeAS400)),
                    HttpUtility.UrlEncode(partner.Code),
                    HttpUtility.UrlEncode(partner.Name),
                    GetUser(),
                    null,
                    HttpUtility.UrlEncode(partner.CountryName),
                    partner.RepresentativeCode.ParseInt().Value);

                if (result == LablatResult.Notsuspicious) {
                    return null;
                }
                partner.IsInvalid = result == LablatResult.Error;
                return new BlacklistAlert {
                    Partner = partner,
                    Status = (Blacklisting)result
                };
            }
        }

        protected void CheckPartners(IEnumerable<Partner> partners) {
            List<LablatResult> resultList = null;
            var partnerList = partners.Cast<IPartner>().ToList();
            using (var client = ServiceClientFactory.GetClient<ILablat>()) {
                resultList = client.Channel.CheckMultipleParterns(partnerList).ToList();
            }
            var alerts = new List<BlacklistAlert>();
            for (int x = 0; x < partnerList.Count; x++) {
                if (resultList[x] != LablatResult.Notsuspicious) {
                    partnerList[x].IsInvalid = resultList[x] == LablatResult.Error;
                    alerts.Add(new BlacklistAlert {
                        Partner = partnerList[x],
                        Status = (Blacklisting)resultList[x]
                    });
                }
            }

            if (alerts?.Any() ?? false) {
                throw new BlacklistException(alerts);
            }
        }
        #endregion

        protected InfosBaseDto GetInfoBaseAffaire(string codeOffre, string version, string type, string avn, string mode) {
            InfosBaseDto infosBaseAffaire;
            using (var chan = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>()) {
                var CommonOffreClient = chan.Channel;
                infosBaseAffaire = CommonOffreClient.LoadInfosBase(codeOffre, version, type, avn, mode);
            }

            return infosBaseAffaire;
        }

    }
}
