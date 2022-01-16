using Albingia.Kheops.Mvc;
using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Mvc.Common;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.AlbingiaExceptions;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.IOFile;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.AlbSpecificAttribute;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Controllers;
using ALBINGIA.OP.OP_MVC.Models.ModelesContextMenu;
using LightInject;
using OP.WSAS400.DTO.FormuleGarantie;
using OPServiceContract.ITraitementsFinOffre;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;


namespace ALBINGIA.OP.OP_MVC {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication {
        #region variables membres
        public static List<ModeleListItem> _contextMenuUsers = null;
        public static List<ModeleListItem> _contextFlatMenuUsers = null;
        #endregion
        #region Variables d'application


        public static readonly string SPLIT_CONST_FILE = FileContentManager.GetConfigValue("SplitCharsFile");
        public static readonly string URL_HEXAVIA = FileContentManager.GetConfigValue("UrlHexavia");
        public static readonly string URL_BACKHEXAVIA = FileContentManager.GetConfigValue("UrlBackHexavia");

        public static readonly string INTERNAL_APPLICATION_ID =
            FileContentManager.GetConfigValue("InternalApplicationId");

        public static readonly string GARA_BASE_COLOR = FileContentManager.GetConfigValue("GaraBaseColor");
        public static readonly string GARA_INCLUDE_COLOR = FileContentManager.GetConfigValue("GaraIncludeColor");
        public static readonly string GARA_GRANTED_COLOR = FileContentManager.GetConfigValue("GaraGrantedColor");
        public static readonly int PAGINATION_PAGE_SIZE = int.Parse(FileContentManager.GetConfigValue("PageSize"));
        public static readonly int PAGINATION_SIZE = int.Parse(FileContentManager.GetConfigValue("PaginationSize"));
        public static readonly int ALBINDEXLIST = int.Parse(FileContentManager.GetConfigValue("AlbIndexList"));

        public static readonly string SPLIT_CONST_HTML = AlbVerifLockedOfferAttribute.SplitChar = FileContentManager.GetConfigValue("SplitCharsHtml");
        public static readonly string EXCELXML_PARAMPATH = FileContentManager.GetConfigValue("XmlParamPath");
        public static readonly string EXCEL_FILEEXTENTION = FileContentManager.GetConfigValue("ExcelFileExtention");
        //public static readonly string CP_TARGETGENERATEFOLDER = FileContentManager.GetConfigValue("TargetGenerateFolder");
        public static readonly string OS_EXCELTEMPLATEFILE = FileContentManager.GetConfigValue("ExcelTemplateFile");
        public static readonly string APP_VERSION = FileContentManager.GetConfigValue("App_Version");
        public static readonly string URL_VIRTGENDOC = FileContentManager.GetConfigValue("UrlGenDoc");
        //paramètres par défaut Stockage des documents
        public static readonly string URL_STOCKAGE_DOCUMENT_SERVEUR = FileContentManager.GetConfigValue("UrlStockageDocumentServeur");
        public static readonly string URL_STOCKAGE_DOCUMENT_RACINE = FileContentManager.GetConfigValue("UrlStockageDocumentRacine");
        //Paramètres de stockage des contrats
        public static readonly string STORAGE_PREFIX_DIRECTORY = FileContentManager.GetConfigValue("StoragePrefixDirectory");
        public static readonly int STORAGE_MAX_FILES = int.Parse(FileContentManager.GetConfigValue("StorageMaxFiles"));
        public static readonly int STORAGE_NUM_POS_DIRECTORY = int.Parse(FileContentManager.GetConfigValue("StorageNumPosDirectory"));
        //Accès super master
        public static readonly string ACCESS_MENU = FileContentManager.GetConfigValue("AccessMenu");
        //chargement des users login à partir du cache
        public static readonly bool USERSLOGINCACHE = bool.Parse(FileContentManager.GetConfigValue("UsersLoginCache"));
        //Chargement des menus, sous-menus à cacher
        public static readonly string HIDDEN_MENU = FileContentManager.GetConfigValue("HiddenMenu");
        public static readonly string GDMAPPNAME = FileContentManager.GetConfigValue("GdmAppName");

        public static readonly string EXTENSION_CLAUSE = FileContentManager.GetConfigValue("ExtensionClause");

        public static readonly int PAGINATION_PAGE_SIZE_DOC = int.Parse(FileContentManager.GetConfigValue("PageSizeDoc"));
        public static readonly int PAGINATIONSIZE_DOC = int.Parse(FileContentManager.GetConfigValue("PaginationSizeDoc"));

        //Paramètres d'url de l'outil Magnetic
        public static readonly string MAGNETIC_URL = FileContentManager.GetConfigValue("MagneticUrl");
        //Paramètres d'url xml de l'outil Magnetic
        public static readonly string MAGNETIC_URL_XML = FileContentManager.GetConfigValue("MagneticUrlXml");
        //Chemin du Clausier
        public static readonly string CHEMIN_CLAUSIER = FileContentManager.GetConfigValue("CheminClausier");
        //Switch pour le module de gestion documentaire
        public static readonly int SWITCH_MODULE_GESTDOC = int.Parse(FileContentManager.GetConfigValue("SwitchModuleGestDoc"));
        //Liste des personnes pour lesquelles le module est ouvert
        public static readonly string USER_MODULE_GESTDOC_OPEN = FileContentManager.GetConfigValue("UserModuleGestDocOpen");
        //Liste des personnes pour lesquelles le module est fermé
        public static readonly string USER_MODULE_GESTDOC_CLOSE = FileContentManager.GetConfigValue("UserModuleGestDocClose");

        //Switch des Logs en mode HARD
        public static readonly int SWITCH_HARD_LOG = int.Parse(FileContentManager.GetConfigValue("SwitchHardLog"));
        //Nom de l'environnment
        public static readonly string ENVIRONMENT_NAME = FileContentManager.GetConfigValue("EnvironmentName");
        //Switch envoi mail
        public static readonly int SENDMAIL = int.Parse(FileContentManager.GetConfigValue("SendMail"));

        //Liste des personnes pour lesquelles la remise en vigueur est ouverte
        public static readonly string USER_REMVIGUEUR = FileContentManager.GetConfigValue("UserRemVigueur");
        //Liste des personnes autorisées pour le centre équestre
        public static readonly string USER_HORSE = FileContentManager.GetConfigValue("UserHorse");

        // URL d'accés au Portail Partenaire
        public static readonly string URL_PARTENAIRE = FileContentManager.GetConfigValue("UrlPortailPartenaire");
        // Liste des personnes pour lesquelles le Portail Partenaire est ouvert
        public static readonly string USER_PARTENAIRE = FileContentManager.GetConfigValue("UserPortailPartenaire");
        #endregion
        #region proprités
        /// <summary>
        /// Module d'affichage est par defaut
        /// </summary>
        public static bool DefaultDocDisplayModule { get; set; }
        /// <summary>
        /// La session est redémarrée suite à un TimeOut 
        /// </summary>
        public static bool SessionRestarted {
            get;
            set;
        }

        /// <summary>
        /// Liste des garanties de la nature des paramètres de garanties
        /// </summary>
        public static List<ParamNatGarDto> AlbLstParamNatGar {
            get {
                return InformationGeneraleTransverse.GetNaturesParamsGaranties();
            }
        }


        /// <summary>
        /// Liste des garanties de la nature des paramètres de garanties
        /// </summary>

        public static List<ModeleListItem> AlbAllContextMenuUsers {
            get {
                // Liste des garanties de la nature des paramètres de garanties
                return _contextMenuUsers ??
                       (_contextMenuUsers = InformationGeneraleTransverse.GetContextMenuFromDb(AlbOpConstants.GdmAppName));
            }
        }

        /// <summary>
        /// Tous les contexts menu de la base GDM
        /// </summary>
        public static List<ModeleListItem> AlbAllFlatContextMenuUsers {
            get {
                return _contextFlatMenuUsers ??
                    (_contextFlatMenuUsers = InformationGeneraleTransverse.GetFlatContextMenu(AlbOpConstants.GdmAppName));
            }
        }

        /// <summary>
        /// Chemin de génération de la CP
        /// </summary>
        public static string PathGenCP {
            get {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                    var serviceContext = client.Channel;
                    return serviceContext.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.CP.ToString());
                }
            }
        }

        /// <summary>
        /// Chemin de sauvegarde de la clause libre généré
        /// </summary>
        public static string PathGenClauseLibre {
            get {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IFinOffre>()) {
                    var serviceContext = client.Channel;
                    return serviceContext.ReloadPathFile(AlbConstantesMetiers.TypologieDoc.ClauseLibre.ToString());
                }
            }
        }



        #endregion

        public static ICollection<AccesAffaire> ListeAccesAffaires {
            get {
                return (HttpContext.Current.Session[nameof(ListeAccesAffaires)] 
                    ?? (HttpContext.Current.Session[nameof(ListeAccesAffaires)] = new HashSet<AccesAffaire>())) as HashSet<AccesAffaire>;
            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (SWITCH_HARD_LOG == 1) { 
                filters.Add(new PerformanceAttribute());
            }
            filters.Add(new CheckVerrouAttribute(), 0);
            filters.Add(new ModelBaseAssignAttribute());
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
            filters.Add(new RollbackAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ProfileKheopsAttribute());
            filters.Add(new HttpGetNoCacheAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}/{loadEntete}", // URL with parameters
                new {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional,
                    loadEntete = UrlParameter.Optional
                });

            routes.MapRoute(
                "DefaultWithoutId", // Route name
                "{controller}/{action}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Index"
                });

            //routes.MapRoute(
            //    "LoadEntete", // Route name
            //    "{controller}/{action}/{id}/{loadEntete}", // URL with parameters
            //    new {
            //        controller = "ModifierOffre",
            //        action = "Index",
            //        id = string.Empty,
            //        loadEntete = UrlParameter.Optional

            //    });

            //MapRoute pour le téléchargement du fichier.  
            routes.MapRoute("Download", "RisqueInventaire/DownloadFile/{fileName}", new { fileName = string.Empty });

            //MapRoute pour l'export excel.  
            routes.MapRoute("ExportFile", "RisqueInventaire/ExportFile/{concerts}/{fileName}/{columns}/{modeNavig}", new {
                concerts = string.Empty,
                fileName = string.Empty,
                columns = string.Empty,
                modeNavig = UrlParameter.Optional
            });

            //MapRoute pour Fitre clause .  
            routes.MapRoute("FiltreClause", "ChoixClauses/Filtre/{typeOp}/{codeOffre}/{version}/{filtreEtape}/{filtreContext}", new {
                typeOp = string.Empty,
                codeOffre = string.Empty,
                version = string.Empty,
                filtreEtape = string.Empty,
                filtreContext = string.Empty
            });

            routes.MapRoute("Affaire", "{controller}/{action}/{codeAffaire}/{version}/{type}/{numeroAvenant}", new {
                controller = "SyntheseAffaire",
                action = "Index",
                codeAffaire = string.Empty,
                version = string.Empty,
                type = string.Empty,
                numeroAvenant = UrlParameter.Optional
            });
        }

        protected void Application_Start()
        {
            //initialisation api
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryDebugWriter.IsTracingDisabled = ConfigurationManager.AppSettings["AppInsightsTraceDebug"]?.ToLower() == "false";
            var container = InitializeInjection();
            Albingia.Kheops.OP.Application.ServiceMapper.Init();
            MapsterConfigurator.Initialize();

            // initalisation du cache serveur (aynchrone)
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IReferentielPort>()) {
                client.Channel.InitAllCaches();
            }

            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["AppInsightInstrumntationKey"];
            
            //Nom du Journal d'évènement Windows
            AlbLog.AlbWndEventLog = FileContentManager.GetConfigValue("AlbWndEventLog");
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);


#if DEBUG
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.DisableTelemetry = true;
#endif

            //paramètre environnement d'exécution
            AlbOpConstants.UrlWinPageTarget = FileContentManager.GetConfigValue("UrlWinPageTarget");
            AlbOpConstants.ClientWorkEnvironment = FileContentManager.GetConfigValue("EnvironnementDeploiment");
            //Nom de l'application
            AlbOpConstants.ApplicationName = FileContentManager.GetConfigValue("ApplicationName");
            //Code de l'application utilisé dans la gestion des droits
            AlbOpConstants.GdmAppName = FileContentManager.GetConfigValue("GdmAppName");
            //Paramètre du mailing
            AlbOpConstants.MainInfoContent.From = FileContentManager.GetConfigValue("From");
            AlbOpConstants.MainInfoContent.To = FileContentManager.GetConfigValue("To");
            AlbOpConstants.MainInfoContent.CC = FileContentManager.GetConfigValue("CC");
            AlbOpConstants.MainInfoContent.Subject = string.Format("Environnement : {0} - {1}", AlbOpConstants.ClientWorkEnvironment, AlbOpConstants.ClientWorkEnvironment + FileContentManager.GetConfigValue("Subject"));
            //Mode de stockage des traces
            AlbOpConstants.WindowEventLog = bool.Parse(FileContentManager.GetConfigValue("WindowEventLog"));
            AlbOpConstants.FileLog = bool.Parse(FileContentManager.GetConfigValue("FileLog"));
            //Notification par mail
            AlbOpConstants.NotificationMail = bool.Parse(FileContentManager.GetConfigValue("NotificationMail"));
            //Version JS CSS
            AlbOpConstants.JsCsVersion = FileContentManager.GetConfigValue("JsCsVersion");
            AlbOpConstants.OtherBrowsers = bool.Parse(FileContentManager.GetConfigValue("OtherBrowsers"));
            //Constante Upload fichier inventaires
            //AlbOpConstants.UploadPathInventory = FileContentManager.GetConfigValue("UploadPathInventory");
            //Constante Upload fichier de retour
            //AlbOpConstants.UploadParthReturnedDocument = FileContentManager.GetConfigValue("UploadParthReturnedDocument");
            //Constante Upload fichier Documents de gestion
            //AlbOpConstants.UploadPathDocumentGestion = FileContentManager.GetConfigValue("UploadPathDocumentGestionVal");

            //Url Outils de gestions d'affichage des documents
            var displayDocModule = FileContentManager.GetConfigValue("DisplayDocModule");
            DefaultDocDisplayModule = displayDocModule == "UrlDocViewer";
            AlbOpConstants.UrlDocViewer = FileContentManager.GetConfigValue(displayDocModule);

            //Chemin des documents intercalaires
            AlbOpConstants.CheminIntercalaire = FileContentManager.GetConfigValue("CheminIntercalaire");
            //Chemin fichiers Documents Contractuels
            AlbOpConstants.CheminDocContract = FileContentManager.GetConfigValue("CheminDocContract");
            //Url External App
            AlbOpConstants.ExternalApp = FileContentManager.GetConfigValue("ExternalApp");

            AlbOpConstants.IgnoreISBranche = FileContentManager.GetConfigValue("IgnoreISBranche");

            RegisterModelBinders();
            //RegisterValueFactories();
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // MNC
            // Moteur des vues :Razor
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }

        /// <summary>
        /// Initializes the LightInject tool
        /// </summary>
        private static ServiceContainer InitializeInjection()
        {
            var container = new ServiceContainer();
            container.RegisterControllers();
            // register other services
            container.RegisterFrom<CompositionRoot>();
            container.EnableMvc();
            return container;
        }

        private void RegisterModelBinders()
        {
            ModelBinders.Binders.DefaultBinder = new CustomModelBinders.AlbModelBinder();
            ModelBinders.Binders.Add(typeof(decimal), new CustomModelBinders.DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(double), new CustomModelBinders.DoubleModelBinder());

            ModelBinders.Binders.Add(typeof(int?), new CustomModelBinders.NullableModelBinder());
            ModelBinders.Binders.Add(typeof(long?), new CustomModelBinders.NullableModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new CustomModelBinders.NullableModelBinder());
            ModelBinders.Binders.Add(typeof(double?), new CustomModelBinders.NullableModelBinder());
            ModelBinders.Binders.Add(typeof(bool?), new CustomModelBinders.NullableModelBinder());

            ModelBinders.Binders.Add(typeof(Models.Inventaires.InventoryItem), new CustomModelBinders.InventoryItemModelBinder());
            Assembly.GetAssembly(typeof(Folder)).DefinedTypes.Where(x => x.IsValueType).ToList().ForEach(x =>
                ModelBinders.Binders.Add(typeof(Nullable<>).MakeGenericType(x), new CustomModelBinders.NullableModelBinder()));
            Assembly.GetAssembly(typeof(Folder)).DefinedTypes.Where(x => x.IsValueType).ToList().ForEach(x =>
               ModelBinders.Binders.Add(x, new CustomModelBinders.NullableModelBinder()));
        }

        private void RegisterValueFactories()
        {
            // remove default implementation
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            // add our custom one
            ValueProviderFactories.Factories.Add(new CustomValueFactories.JsonNetValueProviderFactory());
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && string.IsNullOrEmpty(AlbSessionHelper.As400User) && HttpContext.Current.Session != null)
            {
                AlbTransverse.SetAs400();
            }

            if (Response.StatusCode == 401 && Request.Url.AbsolutePath.Contains("/favicon.ico"))
            {
                Response.RedirectToRoute(

                    new RouteValueDictionary(new
                    {
                        action = "Index" + "/" + Framework.Common.Constants.AlbOpConstants.REDIRECT_ACCESDENIED_ERROR,
                        controller = "Errors",
                    }));
            }
        }

        protected void Session_Start()
        {
            SessionRestarted = false;
            AlbSessionHelper.ParametresRecherches = null;
            //-----Chargement des IS
            //  CacheIS.SetIsModels();
            //---------------------  Utilisateur AS/400 -------------------------------------
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Errors/Index/" + AlbOpConstants.REDIRECT_ACCESDENIED_ERROR);
                Response.Flush();
                Response.End();
                return;
            }
            //Appel de la méthode privée car AlbTransverse non initialisé
            AlbTransverse.SetAs400();
            CacheUserRights.SetUserRights();
            if (string.IsNullOrEmpty(AlbSessionHelper.As400User))
            {

                Response.Redirect("~/Errors/Index/" + AlbOpConstants.REDIRECT_ACCESDENIED_ERROR);
                Response.Flush();
                Response.End();
                return;
            }
            //-------------------------------------------------------------------------------

            var httpContext = HttpContext.Current;
            if (httpContext.User == null) return;
            FolderController.DeverrouillerAffairesUser();
            AlbLog.Log(string.Format("User : {0} est connecté.\n Date de connection :{1}", httpContext.User.Identity.Name, DateTime.Now.ToString(CultureInfo.CurrentCulture)));
        }

        protected void Session_Stop()
        {
            AlbSessionHelper.ParametresRecherches = null;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception is HttpException httpEx) {
                switch (httpEx.GetHttpCode()) {
                    case 401:
                        Response.Redirect("~/Errors/Index/" + AlbOpConstants.REDIRECT_ACCESDENIED_ERROR);
                        return;
                }
            }

            Exception baseEx = exception.GetBaseException();
            if (baseEx is Albingia.Kheops.Common.BusinessException) {
                return;
            }
            Response.ClearContent();
            if (baseEx.Message.ToLower().Contains("méthode d'action publique 'content' introuvable sur le contrôleur"))
            {
                Server.ClearError();
                return;
            }
            if (baseEx.Message.ToLower().Contains("a public action method 'content' was not found on controller"))
            {
                Server.ClearError();
                return;
            }
            if (baseEx.Message.ToLower().Contains("'/favicon.ico'"))
            {
                Server.ClearError();
                return;
            }

            Response.Write(baseEx.Message);
            var message =
              string.Format(
                "Erreur Fatale - Risque d'arrêt ou de comportement imprévisible<br/>.Date :{0}<br/>Utilisateur:{1}<br/>Message : {2} <br/> Stack trace : {3}",
                DateTime.Now.ToString(CultureInfo.CurrentCulture),
                HttpContext.Current.User.Identity.Name,
                baseEx.Message,
                baseEx.StackTrace);

            new AlbTechException(new Exception(message));
        }

        #region Méthodes privées
        private Dictionary<string, AlbProjectInfo> SetSessionUserValues()
        {
            var activeSessions = new List<String>();
            object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            var info = obj.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance);
            if (info != null)
            {
                var obj2 = (object[])info.GetValue(obj);
                foreach (var elem in obj2)
                {
                    var field = elem.GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field == null) continue;
                    var c2 = (Hashtable)field.GetValue(elem);
                    foreach (DictionaryEntry entry in c2)
                    {
                        object o1 = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
                        if (o1.GetType().ToString() != "System.Web.SessionState.InProcSessionState") continue;
                        var fieldInfo = o1.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fieldInfo == null) continue;
                        var sess = (SessionStateItemCollection)fieldInfo.GetValue(o1);
                        if (sess == null)
                            continue;
                        if (sess["ConnectedUser"] != null && !sess["ConnectedUser"].ToString().ToLower().Contains(AlbSessionHelper.ConnectedUser.ToLower()))
                            continue;
                        if (sess["CurrentFolders"] == null) continue;

                        return (Dictionary<string, AlbProjectInfo>)sess["CurrentFolders"];
                    }
                }
            }

            return null;

        }
        #endregion
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiAction",
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}
