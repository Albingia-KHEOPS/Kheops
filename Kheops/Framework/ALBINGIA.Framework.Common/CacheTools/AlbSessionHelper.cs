using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Models.Common;
using ALBINGIA.Framework.Common.Tools.Models.Common;

namespace ALBINGIA.Framework.Common.CacheTools
{
    /// <summary>
    /// SessionHelper: Cette classe regroupe la déclartion de toutes les variables sessions de l'application.
    /// Il est important de déclarer le même nom de la propriété et la variable de session à encapsuler
    /// (à déclarer dans la propriétés) avec le même nom.
    /// </summary>
    public class AlbSessionHelper
    {
        #region membres  d'une classe
        private static HttpSessionState Session
        {
            get
            {

                return HttpContext.Current?.Session;

            }
        }
        #endregion
        #region Méthodes transverses

        /// <summary>
        /// Met à jour une variable session
        /// </summary>
        /// <param name="name">nom de la variable session</param>
        /// <param name="value">valeur de la variable session</param>
        public static void SetValue(string name, object value)
        {

            Session[name] = value;
        }
        /// <summary>
        /// Renvoie la valeur d'une variable session
        /// </summary>
        /// <param name="name">Nom de la variable session</param>
        /// <returns></returns>
        public static object GetValue(string name)
        {
            return Session[name];
        }
        #endregion
        #region Gestion des cookies

        /// <summary>
        /// Delete le cookie spécifié
        /// </summary>
        /// <param name="cookieName">Nom du cookie</param>

        public static void DelCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] == null) return;
            var albCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1d) };
            HttpContext.Current.Response.Cookies.Add(albCookie);
        }
        #endregion
        /// <summary>
        /// Retourne l'utulisateur Connecté
        /// </summary>
        public static string ConnectedUser
        {
            get
            {
                if (Session == null)
                    return null;
                if (Session["ConnectedUser"] == null)
                {
                    Session["ConnectedUser"] = As400User;
                }

                return Session["ConnectedUser"]?.ToString();
            }

        }

        internal static string UserLogin
        {
            get
            {
                string user = "anonymous";
                var session = HttpContext.Current?.Session;
                if (session?["___userLogin___"] != null)
                {
                    user = (String)session["___userLogin___"];
                }
                else if (HttpContext.Current?.User?.Identity?.IsAuthenticated == true)
                {
                    user = HttpContext.Current?.User?.Identity?.Name;
                    if (session != null)
                    {
                        session["___userLogin___"] = HttpContext.Current?.User?.Identity?.Name;
                    }
                }
                return user;
            }
        }

        /// <summary>
        /// Identifiant 400 de l'utilisateur connecté
        /// </summary>
        public static string As400User
        {
            get
            {
                return Session?["AS400User"]?.ToString();
            }
            set
            {
                if (Session != null) { Session["AS400User"] = value; }
            }
        }

        /// <summary>
        /// l'affaire en cours
        /// </summary>
        public static Dictionary<FolderKey, AlbProjectInfo> CurrentFolders
        {
            get
            {
                if (Session["CurrentFolders"] == null)
                {
                    Session["CurrentFolders"] = new Dictionary<FolderKey, AlbProjectInfo>();
                }

                return (Dictionary<FolderKey, AlbProjectInfo>)Session["CurrentFolders"];
            }
            set
            {
                Session["CurrentFolders"] = value;
            }
        }

        /// <summary>
        /// Message d'erreurs Globale
        /// </summary>
        public static string MessageErreurEcran
        {
            get
            {
                return (string)Session["MessageErreurEcran"];
            }
            set
            {
                Session["MessageErreurEcran"] = value;
            }
        }
        /// <summary>
        /// Sauvegarde dans une variable de session l'ecran courant (le nom
        /// de l'écran c'est le nom  du controlleur)
        /// </summary>
        public static NavigationTrace CurrentScreen
        {
            get
            {
                return (NavigationTrace)Session["CurrentScreen"];
            }
            set
            {
                Session["CurrentScreen"] = value;
            }
        }
        #region Profile Behaviour

        //public static bool IsReadOnlyFolder(string guid = "", string currentFolder = "")
        //{
        //    string pageGuid = guid;
        //    if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(currentFolder)) {
        //        return false;
        //    }

        //    if (guid.Contains("tabGuid")) {
        //        pageGuid = guid.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1];
        //    }

        //    if (CurrentFolders.TryGetValue(new FolderKey(ConnectedUser, pageGuid, currentFolder), out var folder) && folder != null) {
        //        if (folder.ReadOnlyFolder) {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static bool IsUserTabFolderLock(string guid, string currentFolder)
        {
            var pageGuid = guid;
            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(currentFolder))
            {
                return false;
            }
            if (guid.Contains("tabGuid"))
            {
                pageGuid = guid.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1];
            }

            var keyUserInfo = pageGuid + "_" + ConnectedUser + "_" + currentFolder;
            if (currentFolder == null || CurrentFolders.Count == 0)
            {
                return false;
            }
            if (CurrentFolders.TryGetValue(new FolderKey(ConnectedUser, pageGuid, currentFolder), out AlbProjectInfo projectInfo))
            {
                // tester si un utilisateur consulte l'offre vérrouillée
                if (projectInfo.LockUser.ContainsChars() && !projectInfo.IsUserLockUser)
                {
                    return true;
                }
            }

            // tester si le même utilisateur consulte la même offre dans une autre tabulation du navigateur
            return CurrentFolders.Any(x => x.Key.User == ConnectedUser && x.Key.FullIdentifier == currentFolder
                && x.Value.LockUser == ConnectedUser && x.Key.TabGuid != pageGuid);
        }

        /// <summary>
        /// Indique que si l'offre consulté ou l'utilisateur sont en lecture seule
        /// </summary>
        /// <returns></returns>
        public static bool IsReadOnly(string guid = "", string currentFolder = "") {
            
            if (!string.IsNullOrEmpty(currentFolder)) {
                currentFolder = currentFolder.Trim();
            }
            var pageGuid = guid;
            if (string.IsNullOrEmpty(guid) || string.IsNullOrEmpty(currentFolder))
            {
                return false;
            }
            if (guid.Contains("tabGuid"))
            {
                pageGuid = guid.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1];
            }

            //var keyUserInfo = pageGuid + "_" + ConnectedUser + "_" + currentFolder;
            var key = new FolderKey(ConnectedUser, pageGuid, currentFolder);
            if (CurrentFolders.TryGetValue(key, out AlbProjectInfo projectInfo))
            {

                // tester le role utilisateur
                // tester si l'offre est vérrouillée
                // tester si l'offre est en mode consultation
                if (projectInfo.ReadOnlyUser || projectInfo.ReadOnlyFolder /*|| projectInfo.ModeConsultationEcran */ && !projectInfo.ModifHorsAvenant)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsModfiHorsAvenant(string guid, string currentFolder)
        {
            if (currentFolder.IsEmptyOrNull())
            {
                return false;
            }
            string pageGuid = guid;
            if (guid?.StartsWith("tabGuid") == true)
            {
                pageGuid = guid.Split(new[] { "tabGuid" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
            }

            if (CurrentFolders.Any())
            {
                var keyUserInfo = new FolderKey(ConnectedUser, pageGuid, currentFolder);
                if (pageGuid.IsEmptyOrNull())
                {
                    keyUserInfo = CurrentFolders.Keys.FirstOrDefault(k => k.FullIdentifier == currentFolder);
                    if (keyUserInfo != null)
                    {
                        return CurrentFolders[keyUserInfo].ModifHorsAvenant;
                    }
                }
                else if (CurrentFolders.TryGetValue(keyUserInfo, out var info) && info != null)
                {
                    return info.ModifHorsAvenant;
                }
            }
            return false;
        }

        public static bool IsReadOnlyScreen(string guid, string currentFolder)
        {
            var pageGuid = guid;
            if (string.IsNullOrEmpty(guid))
            {
                return false;
            }
            if (guid.Contains("tabGuid"))
            {
                pageGuid = guid.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1];
            }

            if (currentFolder == null || CurrentFolders.Count == 0) return false;
            if (CurrentFolders.TryGetValue(new FolderKey(ConnectedUser, pageGuid, currentFolder), out var info) && info?.ModeConsultationEcran == true)
            {
                return true;
            }

            return false;
        }

        public static AlbProjectInfo GetFolderByTabGuid(string tabGuid)
        {
            AlbProjectInfo result = null;

            if (CurrentFolders.Any() && tabGuid.ContainsChars())
            {
                var key = CurrentFolders.Keys.FirstOrDefault(k => k.TabGuid == tabGuid);
                if (key != null)
                {
                    result = CurrentFolders[key];
                }
            }

            return result;
        }
        #endregion
        #region Garanties
        public static Dictionary<string, dynamic> ConditionsTarifairesUtilisateurs
        {
            get
            {

                if (Session["ConditionsTarifairesUtilisateurs"] == null)
                    Session["ConditionsTarifairesUtilisateurs"] = new Dictionary<string, dynamic>();
                return (Dictionary<string, dynamic>)Session["ConditionsTarifairesUtilisateurs"];
            }

            set { Session["ConditionsTarifairesUtilisateurs"] = value; }
        }
        public static Dictionary<string, dynamic> FormulesGarantiesUtilisateurs
        {
            get
            {
                if (Session["FormulesGarantiesUtilisateurs"] == null)
                    Session["FormulesGarantiesUtilisateurs"] = new Dictionary<string, dynamic>();
                return (Dictionary<string, dynamic>)Session["FormulesGarantiesUtilisateurs"];
            }

            set { Session["FormulesGarantiesUtilisateurs"] = value; }
        }
        public static Dictionary<string, dynamic> FormulesGarantiesSaveUtilisateurs
        {
            get
            {
                if (Session["FormulesGarantiesSaveUtilisateurs"] == null)
                    Session["FormulesGarantiesSaveUtilisateurs"] = new Dictionary<string, dynamic>();
                return (Dictionary<string, dynamic>)Session["FormulesGarantiesSaveUtilisateurs"];
            }

            set { Session["FormulesGarantiesSaveUtilisateurs"] = value; }
        }
        public static Dictionary<string, dynamic> FormulesGarantiesHistoUtilisateurs
        {
            get
            {
                if (Session["FormulesGarantiesHistoUtilisateurs"] == null)
                    Session["FormulesGarantiesHistoUtilisateurs"] = new Dictionary<string, dynamic>();
                return (Dictionary<string, dynamic>)Session["FormulesGarantiesHistoUtilisateurs"];
            }

            set { Session["FormulesGarantiesHistoUtilisateurs"] = value; }
        }
        #endregion
        #region Engagement
        public static Dictionary<string, dynamic> EngagementPeriodesUtilisateurs
        {
            get
            {

                if (Session["EngagementPeriodesUtilisateurs"] == null)
                    Session["EngagementPeriodesUtilisateurs"] = new Dictionary<string, dynamic>();
                return (Dictionary<string, dynamic>)Session["EngagementPeriodesUtilisateurs"];
            }

            set { Session["EngagementPeriodesUtilisateurs"] = value; }
        }
        #endregion
        #region CoAssureurs
        public static Dictionary<string, dynamic> CoAssureursUtilisateurs
        {
            get
            {

                if (Session["CoAssureursUtilisateurs"] == null)
                    Session["CoAssureursUtilisateurs"] = new Dictionary<string, dynamic>();
                return (Dictionary<string, dynamic>)Session["CoAssureursUtilisateurs"];
            }

            set { Session["CoAssureursUtilisateurs"] = value; }
        }
        #endregion
        #region Menu Contextuel

        public static Dictionary<string, dynamic> ContextMenuUtilisateurs
        {
            get
            {
                if (Session["ContextMenuUtilisateurs"] == null)
                    Session["ContextMenuUtilisateurs"] = new Dictionary<string, dynamic>();
                return (Dictionary<string, dynamic>)Session["ContextMenuUtilisateurs"];
            }
            set { Session["ContextMenuUtilisateurs"] = value; }
        }

        #endregion
        #region Paramètres de recherche

        public static dynamic ParametresRecherches
        {
            get
            {
                return Session["ParamtresRecherches"];// ?? (Session["ParamtresRecherches"] = Activator.CreateInstance<dynamic>());
            }
            set { Session["ParamtresRecherches"] = value; }
        }

        #endregion

        #region AttestationCache

        public static Dictionary<string, dynamic> AttestationGarUtilisateurs
        {
            get
            {
                if (Session["AttestationGarUtilisateurs"] == null)
                    Session["AttestationGarUtilisateurs"] = new Dictionary<string, dynamic>();

                return (Dictionary<string, dynamic>)Session["AttestationGarUtilisateurs"];
            }
            set { Session["AttestationGarUtilisateurs"] = value; }
        }

        #endregion

        #region Sauvegarde des entités
        public static dynamic GetCurrentEntityById(string id)
        {
            dynamic result = null;
            try
            {

                if (!string.IsNullOrEmpty(id))
                {
                    if (Session[id] != null)
                    {
                        result = Session[id];
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;


        }
        public static void DeleteCurrentEntityById(string id)
        {

            try
            {

                if (!string.IsNullOrEmpty(id))
                {
                    if (Session[id] != null)
                    {
                        Session[id] = null;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }



        }
        public static void SetCurrentEntityById(string id, dynamic value)
        {

            try
            {

                if (!string.IsNullOrEmpty(id))
                {
                    DeleteCurrentEntityById(id);
                    Session[id] = value;

                }
            }
            catch (Exception)
            {

                throw;
            }



        }

        #endregion
    }

    #region Classe info sessions

    /// <summary>
    /// Informations génèrales de l'utilisateur encours
    /// </summary>
    public class AlbProjectInfo
    {
        public string TabGuid { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Folder { get; set; }
        public string State { get; set; }
        public bool ReadOnlyUser { get; set; }
        public bool ReadOnlyFolder { get; set; }
        public string LockUser { get; set; }
        public bool FirstScreen { get; set; }
        public bool ModeConsultationEcran { get; set; }
        public bool ModifHorsAvenant { get; set; }

        public string CurrentFlow { get; set; }

        public bool IsUserLockUser => LockUser == UserName;

        public string FlowInitAction { get; set; }
    }
    ///// <summary>
    ///// Dictionnaires des informations génèrales encours
    ///// </summary>
    //[Serializable]
    //public class AlbProjectFolderInfos : IEnumerable
    //{
    //    public Dictionary<FolderKey, AlbProjectInfo> CurrentInfoUser
    //    {
    //        get { return AlbSessionHelper.CurrentFolders; }
    //    }
    //    public AlbProjectInfo this[FolderKey key] {
    //        get {
    //            return AlbSessionHelper.CurrentFolders.TryGetValue(key, out var info) ? info : null;
    //        }
    //        set {
    //            if (AlbSessionHelper.CurrentFolders[key] == null) {
    //                AlbSessionHelper.CurrentFolders[key] = new AlbProjectInfo();
    //            }
    //            AlbSessionHelper.CurrentFolders[key] = value;
    //        }
    //    }
    //    public IEnumerator GetEnumerator()
    //    {
    //        return AlbSessionHelper.CurrentFolders.GetEnumerator();
    //    }
    //}
    #endregion
}
