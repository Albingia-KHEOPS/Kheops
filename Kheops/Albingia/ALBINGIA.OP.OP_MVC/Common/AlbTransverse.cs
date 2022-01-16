using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesContextMenu;
using ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque;
using OP.WSAS400.DTO.Ecran.DetailsObjetRisque;
using OPServiceContract;
using OPServiceContract.IAdministration;
using OPServiceContract.IClausesRisquesGaranties;
using OPServiceContract.ICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Common
{
    public class AlbTransverse
    {

        #region Constantes Documents
        public const string DOCUMENT_RETOUR = "DocRetours";
        public const string DOCUMENT_INV = "Inventaires";
        public const string DOCUMENT_GESTION = "DocumentsGestion";
        #endregion
        #region Documents
        /// <summary>
        /// Retourne la clé du webconfig du type de document correspondant
        /// </summary>
        /// <param name="typeDocument"> type du document</param>
        /// <returns></returns>
        //[Obsolete]
        //public static string GetConfigPathDocument(string typeDocument)
        //{
        //    switch (typeDocument)
        //    {
        //        case DOCUMENT_RETOUR:
        //            return AlbOpConstants.UploadParthReturnedDocument;
        //        case DOCUMENT_INV:
        //            return AlbOpConstants.UploadPathInventory;
        //        case DOCUMENT_GESTION:
        //            return AlbOpConstants.UploadPathDocumentGestion;
        //        default:
        //            return string.Empty;

        //    }
        //}


        #endregion
        #region Lecture seule
        public static bool GetIsReadOnly(string guid = "", string currentFolder = "")
        {

            return AlbSessionHelper.IsReadOnly(guid, currentFolder);
        }
        public static bool GetIsModifHorsAvn(string guid , string currentFolder)
        {
            return AlbSessionHelper.IsModfiHorsAvenant(guid, currentFolder);
        }

        public static bool GetIsReadOnlyScreen(string guid = "", string currentFolder = "")
        {

            return AlbSessionHelper.IsReadOnlyScreen(guid, currentFolder);
        }
        #endregion
        #region Verouillage
        public static bool HasUserOffreLocked(Guid? currentTab)
        {
            bool bypassTab = currentTab.GetValueOrDefault() == default;
            var listeAcces = MvcApplication.ListeAccesAffaires.Where(x => x.VerrouillageEffectue && x.TabGuid != currentTab).ToArray();
            IEnumerable<VerrouAffaire> verrousDB = null;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>()) {
                verrousDB = client.Channel.GetUserLocks();
            }
            if (verrousDB.Count() > MvcApplication.ListeAccesAffaires.Count()) {
                return true;
            }
            return verrousDB.Any(x => listeAcces.Any(a => a.Code.ToIPB() == x.AffaireId.CodeAffaire && a.Version == x.AffaireId.NumeroAliment));
            //var lstOffreCache = CacheVerouillage.GetAllCache(MvcApplication.SPLIT_CONST_HTML);
            //if (lstOffreCache != null && lstOffreCache.Count > 0)
            //{
            //    foreach (var offre in lstOffreCache)
            //    {
            //        var numAvn = "";
            //        if (!string.IsNullOrEmpty(id) && id.Contains("AVN"))
            //        {
            //            numAvn = InformationGeneraleTransverse.GetAddParamValue(id.Split(new[] { "|||" }, StringSplitOptions.None)[1], AlbParameterName.AVNID);
            //        }

            //        var tInfoOffre = offre.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
            //        if (id == null && AlbSessionHelper.ConnectedUser == tInfoOffre[0])
            //            return true;

            //        var numAvnCache = String.Empty;
            //        var typeCache = String.Empty;

            //        var tabCache = tInfoOffre[3].Split('_');
            //        if (tabCache.Length > 1)
            //        {
            //            typeCache = tabCache[0];
            //            numAvnCache = tabCache[1];
            //        }

            //        string[] idElems = id.Split('_');

            //        if (AlbSessionHelper.ConnectedUser == tInfoOffre[0]
            //            && (
            //                idElems[0] != tInfoOffre[1] ||
            //                idElems[1] != tInfoOffre[2] ||
            //                idElems[2] != typeCache ||
            //                !string.IsNullOrWhiteSpace(numAvn) && numAvn != numAvnCache
            //            ))
            //        {
            //            return true;
            //        }
            //    }
            //}
            //return false;
        }
        public static bool IsOffreVerrrouille(string offre, string version, string type, string numAvn)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var vBackOffice=client.Channel;
                return vBackOffice.IsOffreVeruouille(offre, version, type, numAvn);
            }
        }
        #endregion
        #region Session
        public static void InitSessionMemory()
        {
            AlbSessionHelper.EngagementPeriodesUtilisateurs = null;
        }

        public static void SetAs400()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) {
                return;
            }
            var userWin = HttpContext.Current.User.Identity.Name.Trim().ToLower();
            if (!MvcApplication.USERSLOGINCACHE)
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<ICommonOffre>())
                {
                    AlbSessionHelper.As400User = client.Channel.GetAs400User(userWin.Split(new [] { '\\' }, StringSplitOptions.None).LastOrDefault());
                    return;
                }
            }

            //CacheUsersLogin.SetUserLogin();
            if (CacheUsersLogin.AllUsersLogin == null || !CacheUsersLogin.AllUsersLogin.Any())
            {
                AlbSessionHelper.As400User = string.Empty;
                return;
            }
            var windowsUser = (userWin.Contains("\\")
                ? userWin.Split(new[] { "\\" }, StringSplitOptions.None)[1]
                : userWin).Trim().ToLower();

            Models.ModelesUsersLogin.UsersLogin[] usersLogin = CacheUsersLogin.AllUsersLogin.ToArray();
            var curentUserLogin = usersLogin.FirstOrDefault(elm =>
                elm.UserWin.Trim().ToLower() == windowsUser);


            AlbSessionHelper.As400User = curentUserLogin == null ? string.Empty : curentUserLogin.UserLogin.Trim();
        }
        #endregion
        #region DataType
        /// <summary>
        /// Intialisation DateType
        /// Saisie, Effet
        /// </summary>
        /// <returns></returns>
        public static List<AlbSelectListItem> InitDateType
        {
            get
            {
                var etats = new List<AlbSelectListItem>();
                etats.Insert(0, new AlbSelectListItem { Value = "Saisie", Text = "Saisie ou Accord", Selected = false });
                etats.Insert(1, new AlbSelectListItem { Value = "Effet", Text = "Effet", Selected = false });
                etats.Insert(2, new AlbSelectListItem { Value = "MAJ", Text = "Mise à jour", Selected = true });
                etats.Insert(3, new AlbSelectListItem { Value = "Creation", Text = "Création", Selected = false });
                return etats;
            }
        }
        #endregion
        #region Nettoyage menus

        public static List<ModeleListItem> CleanMenus(List<ModeleListItem> menus)
        {
            List<ModeleListItem> toReturnList = menus;

            string hiddenMenus = MvcApplication.HIDDEN_MENU;
            string[] tMenus = hiddenMenus.Split(';');

            foreach (var menu in tMenus)
            {
                if (!string.IsNullOrEmpty(menu))
                {
                    var subMenu = menu.Split(new[] { "||" }, StringSplitOptions.None);
                    var isSubMenu = subMenu.Length > 1;
                    if (isSubMenu) {
                        if (Enum.TryParse(subMenu[0].Substring(1).ToUpper(), out AlbContextMenu mEnum)
                            && mEnum.IsIn(AlbContextMenu.CREER, AlbContextMenu.OFFCONT, AlbContextMenu.AVENANT)) {
                            var filteredList = toReturnList.FindAll(el => el.text == mEnum.DisplayName());
                            filteredList.ForEach(group => {
                                for (int i = 1; i < subMenu.Length; i++) {
                                    group.items.RemoveAll(el => el.alias == subMenu[i]);
                                }
                                var cleanMenu = group.items.Count > 0;
                                if (cleanMenu == false) {
                                    toReturnList.RemoveAll(el => el.text == mEnum.DisplayName());
                                }
                            });
                        }
                    }
                    else {
                        if (Enum.TryParse(menu.Substring(1).ToUpper(), out AlbContextMenu mEnum)
                            && mEnum.IsIn(AlbContextMenu.CREER, AlbContextMenu.OFFCONT, AlbContextMenu.AVENANT)) {
                            toReturnList.RemoveAll(el => el.text == mEnum.DisplayName());
                        }
                    }
                }
            }
            return toReturnList;
        }

        #endregion
        #region Gestion des Quittances
        public static bool GetIsDisplayQuittance(HttpContextBase context, bool isAvenant = false, bool isEntete = false)
        {
            if (isAvenant && isEntete)
                return true;

            if (isAvenant && context.Request.UrlReferrer.AbsoluteUri.ToLower() != NomsInternesEcran.RechercheSaisie.ToString().ToLower())
                return false;
            return true;

        }
        #endregion
        #region Activites
        public static ModeleRechercheActivite GetListActivites(string codeBranche, string codeCible, int pageNumber, string code, string nom)
        {
            int PageSize = 20;
            ModeleRechercheActivite activiteModele = new ModeleRechercheActivite();
            var pagenumber = pageNumber;
            var StartLine = ((pagenumber - 1) * PageSize) + 1;
            var EndLine = (pagenumber) * PageSize;
            var LineCount = MvcApplication.PAGINATION_SIZE;

            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IRisquesGaranties>())
            {
                var serviceContext=client.Channel;
                RechercheActiviteDto result = serviceContext.GetActivites(code, codeBranche, codeCible, nom, StartLine, EndLine);
                activiteModele.ListActivites = new List<ModeleLigneActivite>();
                if (result != null && result.ListActivites.Any())
                {
                    result.ListActivites.ForEach(m => activiteModele.ListActivites.Add((ModeleLigneActivite)m));
                    activiteModele.concept = result.ListActivites[0].Concept;
                    activiteModele.famille = result.ListActivites[0].Famille;
                }
                int totalLines = 0;
                totalLines = result.ResultCount < LineCount ? result.ResultCount : LineCount;
                activiteModele.NbCount = result.ResultCount;
                activiteModele.StartLigne = StartLine;
                activiteModele.EndLigne = EndLine;
                activiteModele.PageNumber = pagenumber;
                activiteModele.LineCount = totalLines;

                return activiteModele;
            }
        }
        #endregion

    }
}