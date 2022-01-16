using Albingia.Kheops.OP.Application.Port.Driver;
using Albingia.Kheops.DTO;
using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Constants;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Common;
using ALBINGIA.OP.OP_MVC.Models.ModelesOffresVerrouillees;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
using OP.WSAS400.DTO.VerouillageOffres;
using OPServiceContract;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Controllers
{
    public class OffresVerrouilleesController : ControllersBase<ModeleOffresVerrouilleesPage>
    {
        public ActionResult Index()
        {
            model.PageTitle = "Liste des offres verrouillees";
            model.EtapeEcran = AlbConstantesMetiers.ETAPE_ECRAN_BACKOFFICE;
            LoadInfoPage();
            model.NbOffreBdd = model.OffresVerrouillees.Count;
            model.NbOffreCache = LoadOffreVerrouilleeCache().Count();
            DisplayBandeau();
            return View(model);
        }

        public ActionResult SupprimerOffres(string numerosOffres)
        {
            LoadInfoPage();
            if (!string.IsNullOrEmpty(numerosOffres))
            {
                var splitTab = numerosOffres.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                foreach (var numeroOffre in splitTab)
                {
                    DeverouillerOffre(numeroOffre);
                }
            }
            LoadInfoPage();
            return PartialView("ListeOffresVerrouillees", model.OffresVerrouillees);
        }

        private void DeverouillerOffre(string numeroOffre, bool useContentData = true, string tabGuid = "")
        {
            if (string.IsNullOrEmpty(numeroOffre))
                return;
            var elem = numeroOffre.Split(new[] { "||" }, StringSplitOptions.None);
            ModeleOffreVerrouillee offreVerrouillee = null;
            bool isModifHorsAvn = false;
            if (useContentData)
            {
                offreVerrouillee = model.OffresVerrouillees.FirstOrDefault(x => x.NumOffre == elem[0] && x.Version == elem[1] && x.NumAvenant == Convert.ToInt32(elem[3]));
                if (offreVerrouillee != null)
                {
                    model.OffresVerrouillees.Remove(offreVerrouillee);
                    var folder = string.Format("{0}_{1}_{2}_{3}", offreVerrouillee.NumOffre, offreVerrouillee.Version, offreVerrouillee.Type, offreVerrouillee.NumAvenant.ToString(CultureInfo.InvariantCulture));
                    isModifHorsAvn = GetIsModifHorsAvn(tabGuid, folder);
                }
            }
            CommonVerouillage.UnlockFolderByName(numeroOffre, tabGuid, isModifHorsAvn);
        }

        [HttpPost]
        public void UnlockFolder(Folder folder, string tabGuid)
        {
            try
            {
                CommonVerouillage.UnlockFolderByName(folder.FullIdentifier, GetSurroundedTabGuid(tabGuid));
            }
            catch
            {
                try { AlbLog.Warn($"Impossible de déverrouiller {folder.BuildId(" ")}"); } catch { }
            }
        }

        /// <summary>
        /// Dévérrouille toutes les offres ayant le Guid passé en paramètre
        /// </summary>
        /// <param name="tabGuid">Guid de la page</param>
        public void DeverouillerUserOffre(string tabGuid = "")
        {
            if (string.IsNullOrEmpty(tabGuid))
            {
                return;
            }
            FolderController.DeverrouillerAffaire(tabGuid);
            List<FolderKey> keysToDelete = null;
            AlbSessionHelper.CurrentFolders.ToList().ForEach(infoFolder => {
                if (infoFolder.Key.TabGuid != tabGuid.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1])
                {
                    return;
                }
                if (!infoFolder.Value.IsUserLockUser)
                {
                    if (keysToDelete == null)
                    {
                        keysToDelete = new List<FolderKey>();
                    }
                    keysToDelete.Add(infoFolder.Key);
                    return;
                }

                var elems = infoFolder.Value.Id.Split('_');
                CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.DeleteFromCache(
                    elems[1], elems[2].Trim(), elems[3], elems[4] + "_" + elems[5],
                    MvcApplication.SPLIT_CONST_HTML);
                if (keysToDelete == null)
                {
                    keysToDelete = new List<FolderKey>();
                }
                keysToDelete.Add(infoFolder.Key);
                if (!infoFolder.Value.ReadOnlyFolder || infoFolder.Value.ModifHorsAvenant || infoFolder.Value.CurrentFlow == FlowName.Engagement.ToString())
                {
                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var voletsBlocsCategoriesClient = client.Channel;
                        voletsBlocsCategoriesClient.SupprimerOffreVerouillee(elems[2].Trim(), elems[3], elems[4], elems[5], GetUser(), string.Empty, false, infoFolder.Value.ModifHorsAvenant, false);
                    }
                }
            });

            if (keysToDelete?.Any() == true)
            {
                keysToDelete.ForEach(key => AlbSessionHelper.CurrentFolders.Remove(key));
            }
        }

        public ActionResult RefreshPage()
        {
            LoadInfoPage();
            return PartialView("ListeOffresVerrouillees", model.OffresVerrouillees);
        }
        public ActionResult FiltrerOffres(string utilisateurFiltre, string dateDebutFiltre, string dateFinFiltre)
        {
            LoadInfoPage();

            if (!string.IsNullOrEmpty(utilisateurFiltre))
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Utilisateur.Contains(utilisateurFiltre));
            }
            if (!string.IsNullOrEmpty(dateDebutFiltre))
            {
                var splitTab = dateDebutFiltre.Split('/');
                if (splitTab.Length == 3)
                {
                    model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.DateVerouillage >= new DateTime(int.Parse(splitTab[2]), int.Parse(splitTab[1]), int.Parse(splitTab[0])));
                }
            }
            if (!string.IsNullOrEmpty(dateFinFiltre))
            {
                var splitTab = dateFinFiltre.Split('/');
                if (splitTab.Length == 3)
                {
                    model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.DateVerouillage <= new DateTime(int.Parse(splitTab[2]), int.Parse(splitTab[1]), int.Parse(splitTab[0])));
                }
            }

            return PartialView("ListeOffresVerrouillees", model.OffresVerrouillees);
        }

        public ActionResult AjouterOffreVerouille()
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient = client.Channel;
                voletsBlocsCategoriesClient.AjouterOffreVerouille("PRODU", "O", "30412", 0, 0, 0, 0, "", "Contrat", "GESTION", "O", GetUser(), "test");
            }
            return PartialView("ListeOffresVerrouillees", model.OffresVerrouillees);
        }

        public ActionResult GetOffresVerouillees(bool typeOffreO, bool typeOffreP, string numOffre, string version, string utilisateur, DateTime? dateVerouillageDebut, DateTime? dateVerouillageFin)
        {
            LoadInfoPage();

            //var dateDebut = dateVerouillageDebut.HasValue ? AlbConvert.ConvertDateToInt(dateVerouillageDebut).Value.ToString(CultureInfo.CurrentCulture) : string.Empty;
            //var dateFin = dateVerouillageFin.HasValue ? AlbConvert.ConvertDateToInt(dateVerouillageFin).Value.ToString(CultureInfo.CurrentCulture) : string.Empty;

            if (typeOffreO && typeOffreP)
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Type == "O" || x.Type == "P");
            }
            else if (typeOffreO)
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Type == "O");
            }
            else if (typeOffreP)
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Type == "P");
            }

            if (!string.IsNullOrEmpty(numOffre))
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.NumOffre == numOffre);
            }

            if (!string.IsNullOrEmpty(version))
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Version == version);
            }
            if (!string.IsNullOrEmpty(utilisateur))
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Utilisateur.Contains(utilisateur.ToUpper()));
            }

            if (dateVerouillageDebut.HasValue)
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => !x.DateVerouillage.HasValue || x.DateVerouillage.Value.Date >= dateVerouillageDebut.Value.Date);
            if (dateVerouillageFin.HasValue)
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => !x.DateVerouillage.HasValue || x.DateVerouillage.Value.Date <= dateVerouillageFin.Value.Date);

            //if (!string.IsNullOrEmpty(dateDebut))
            //{
            //    string[] splitTab = dateDebut.Split('/');
            //    if (splitTab.Length == 3)
            //    {
            //        model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.DateVerouillage >= new DateTime(int.Parse(splitTab[2]), int.Parse(splitTab[1]), int.Parse(splitTab[0])));
            //    }
            //}
            //if (!string.IsNullOrEmpty(dateFin))
            //{
            //    var splitTab = dateFin.Split('/');
            //    if (splitTab.Length == 3)
            //    {
            //        model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.DateVerouillage <= new DateTime(int.Parse(splitTab[2]), int.Parse(splitTab[1]), int.Parse(splitTab[0])));
            //    }
            //}
            return PartialView("ListeOffresVerrouillees", model.OffresVerrouillees);

        }

        [ErrorHandler]
        public ActionResult LoadOffreCache()
        {
            return PartialView("ListeOffreVerrouileesCache", LoadOffreVerrouilleeCache());
        }

        [ErrorHandler]
        public ActionResult DeleteOffreCache(string offres, string codeOffre = "", string version = "", string type = "", string numAvenant = "", int onlyUser = 0)
        {
            if (!string.IsNullOrEmpty(offres))
            {
                var tOffres = offres.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                var elemKeys = new List<FolderKey>();
                foreach (var offre in tOffres)
                {
                    var elem = offre.Split('_');
                    CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.DeleteFromCache(
                        UserId, elem[0].Trim(), elem[1], elem[2] + "_" + elem[3],
                        MvcApplication.SPLIT_CONST_HTML);

                    using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                    {
                        var voletsBlocsCategoriesClient = client.Channel;
                        var numAvn = string.IsNullOrEmpty(elem[3]) ? "0" : elem[3];
                        var folder = string.Format("{0}_{1}_{2}_{3}", elem[0], elem[1], elem[2], numAvn);
                        var isModifHorsAvn = GetIsModifHorsAvn(string.Empty, folder);
                        voletsBlocsCategoriesClient.SupprimerOffreVerouillee(elem[0].Trim(), elem[1], elem[2], elem[3], GetUser(), string.Empty, false, isModifHorsAvn, false);
                    }

                    if (AlbSessionHelper.CurrentFolders?.Any() == true)
                    {
                        elemKeys.Add(new FolderKey(
                            AlbSessionHelper.ConnectedUser,
                            string.Empty,
                            string.Join("_", elem.Take(4).Select(x => x.Trim()))));
                    }
                }

                if (elemKeys.Any())
                {
                    elemKeys.ForEach(x => {
                        var folderKeys = AlbSessionHelper.CurrentFolders.Keys.Where(k => x.EqualsIgnoreGuid(k)).ToList();
                        folderKeys.ForEach(k => AlbSessionHelper.CurrentFolders.Remove(k));
                    });
                }
            }

            if (onlyUser == 1)
            {
                return PartialView("ListeOffreVerrouileesCache", LoadOffreVerrouilleeCache(codeOffre, version, type, numAvenant, UserId));
            }
            return PartialView("ListeOffreVerrouileesCache", LoadOffreVerrouilleeCache());
        }

        [ErrorHandler]
        public ActionResult CloseDivOffreCache()
        {
            LoadInfoPage();
            return PartialView("ListeOffresVerrouillees", model.OffresVerrouillees);
        }

        [ErrorHandler]
        public string ReloadNbOffreVerrouillee()
        {
            var offreCache = LoadOffreVerrouilleeCache();
            var nbOffreCache = offreCache.Count.ToString(CultureInfo.InvariantCulture);
            string nbOffreBdd;
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var serviceContext = client.Channel;
                nbOffreBdd = serviceContext.GetOffresVerouillees(false, false, string.Empty,
                                                                   string.Empty,
                                                                   string.Empty, string.Empty, null, null).ToList().Count().ToString(CultureInfo.InvariantCulture);

            }
            return string.Format("{0}_{1}", nbOffreBdd, nbOffreCache);
        }

        [ErrorHandler]
        public ActionResult GetUserOffreLocked(string codeOffre, string version, string type, string numAvn)
        {
            return PartialView("ListeOffreVerrouileesCache", LoadOffreVerrouilleeCache(codeOffre, version, type, numAvn, UserId));
        }

        [HandleJsonError]
        public ActionResult GetVerrousAffaires(Guid? currentTab) {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IAffairePort>()) {
                return PartialView("ListeOffreVerrouileesCache", BuildListeVerrousModel(client.Channel.GetUserLocks(), currentTab));
            }
        }

        public ActionResult Tri(string colName, string modeTri, bool typeOffreO, bool typeOffreP, string numOffre, string version, string utilisateur, DateTime? dateVerouillageDebut, DateTime? dateVerouillageFin)
        {
            LoadInfoPage();
            if (typeOffreO && typeOffreP)
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Type == "O" || x.Type == "P");
            }
            else if (typeOffreO)
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Type == "O");
            }
            else if (typeOffreP)
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Type == "P");
            }

            if (!string.IsNullOrEmpty(numOffre))
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.NumOffre == numOffre);
            }

            if (!string.IsNullOrEmpty(version))
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Version == version);
            }
            if (!string.IsNullOrEmpty(utilisateur))
            {
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => x.Utilisateur.Contains(utilisateur.ToUpper()));
            }

            if (dateVerouillageDebut.HasValue)
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => !x.DateVerouillage.HasValue || x.DateVerouillage.Value.Date >= dateVerouillageDebut.Value.Date);
            if (dateVerouillageFin.HasValue)
                model.OffresVerrouillees = model.OffresVerrouillees.FindAll(x => !x.DateVerouillage.HasValue || x.DateVerouillage.Value.Date <= dateVerouillageFin.Value.Date);

            if (model.OffresVerrouillees.Any())
            {
                if (colName.Trim() == "Code Offre")
                {
                    if (modeTri == "asc")
                        model.OffresVerrouillees = model.OffresVerrouillees.OrderBy(o => o.NumOffre).ToList();
                    else if (modeTri == "desc")
                        model.OffresVerrouillees = model.OffresVerrouillees.OrderByDescending(o => o.NumOffre).ToList();

                }
                else if (colName.Trim() == "Version")
                {
                    if (modeTri == "asc")
                        model.OffresVerrouillees = model.OffresVerrouillees.OrderBy(o => o.Version).ToList();
                    else if (modeTri == "desc")
                        model.OffresVerrouillees = model.OffresVerrouillees.OrderByDescending(o => o.Version).ToList();
                }
            }


            return PartialView("ListeOffresVerrouillees", model.OffresVerrouillees);
        }
        #region Méthodes privées
        private static void SendMailTraceUnlock(string tabGuid)
        {
            if (AlbTools.IsEmptyList(AlbSessionHelper.CurrentFolders.Keys.ToList()))
            {
                return;
            }

            var allCurrentUserFolder = "<b> Offres Utilisateurs consultées <b> :";
            AlbSessionHelper.CurrentFolders
                .Where(el => el.Key.User == AlbSessionHelper.ConnectedUser).ToList()
                .ForEach(elm => allCurrentUserFolder += (elm.Key + "  ||  "));

            var foldersByGuid = AlbSessionHelper.CurrentFolders.Where(el => el.Key.TabGuid == tabGuid.Replace("tabGuid", string.Empty));


            var allCurrentRemovedUserFolder = "<b> Offres Utilisateurs supprimées <b> :";
            foldersByGuid.ToList().ForEach(el => {
                AlbSessionHelper.CurrentFolders.Remove(el.Key);
                allCurrentRemovedUserFolder += el.Key + "  ||  ";
            });

            var allCurrentRestUserFolder = "<b> Offres Utilisateurs restantes <b> :";
            AlbSessionHelper.CurrentFolders
                .Where(el => el.Key.User == AlbSessionHelper.ConnectedUser).ToList()
                .ForEach(elm => allCurrentRestUserFolder += (elm.Key + "  ||  "));

            AlbMailing.SendMail(string.Format(
                "Utilisateur:<b>{0}</b></br><b>{1}<b></br><b>{2}</b></br><b>{3}</b>",
                AlbSessionHelper.ConnectedUser, allCurrentUserFolder, allCurrentRemovedUserFolder,
                allCurrentRestUserFolder));
        }

        protected override void LoadInfoPage(string context = null)
        {
            using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
            {
                var voletsBlocsCategoriesClient = client.Channel;


                List<OffreVerouilleeDto> lstOffreCache =
                  voletsBlocsCategoriesClient.GetOffresVerouillees(false, false, string.Empty,
                                                                   string.Empty,
                                                                   string.Empty, string.Empty, null, null).ToList();
                model.OffresVerrouillees = new List<ModeleOffreVerrouillee>();

                lstOffreCache.ForEach(el => model.OffresVerrouillees.Add((ModeleOffreVerrouillee)el));
            }
        }

        private List<ModeleOffreVerrouillee> BuildListeVerrousModel(IEnumerable<VerrouAffaire> verrousDB, Guid? currentTab) {
            if (verrousDB.Count() == 0) {
                return new List<ModeleOffreVerrouillee>();
            }

            if (currentTab.GetValueOrDefault() == default) {
                return verrousDB.Select(v => new ModeleOffreVerrouillee {
                    NumOffre = v.AffaireId.CodeAffaire,
                    Version = v.AffaireId.NumeroAliment.ToString(),
                    Type = v.AffaireId.TypeAffaire.AsCode(),
                    DateVerouillage = v.Date,
                    NumAvenant = v.AffaireId.NumeroAvenant.GetValueOrDefault(),
                    Utilisateur = UserId
                }).ToList();
            }
            
            var listeAcces = MvcApplication.ListeAccesAffaires.Where(x => x.TabGuid == currentTab && x.VerrouillageEffectue).ToArray();
            return verrousDB
                .Where(v => !listeAcces.Any(x => x.Code.ToIPB() == v.AffaireId.CodeAffaire && x.Version == v.AffaireId.NumeroAliment))
                .Select(v => new ModeleOffreVerrouillee {
                    NumOffre = v.AffaireId.CodeAffaire,
                    Version = v.AffaireId.NumeroAliment.ToString(),
                    Type = v.AffaireId.TypeAffaire.AsCode(),
                    DateVerouillage = v.Date,
                    NumAvenant = v.AffaireId.NumeroAvenant.GetValueOrDefault(),
                    Utilisateur = UserId
                })
                .ToList();
        }
        
        private List<ModeleOffreVerrouillee> LoadOffreVerrouilleeCache(string codeOffre = "", string version = "", string type = "", string numAvenant = "", string userId = "")
        {
            List<ModeleOffreVerrouillee> model = new List<ModeleOffreVerrouillee>();
            var lstOffreCache = CacheVerouillage.GetAllCache(MvcApplication.SPLIT_CONST_HTML);
            if (lstOffreCache != null && lstOffreCache.Count > 0)
            {
                foreach (var offre in lstOffreCache)
                {
                    var tInfoOffre = offre.Split(new[] { MvcApplication.SPLIT_CONST_HTML }, StringSplitOptions.None);
                    var parmPlus = tInfoOffre[3].Split('_');
                    if ((!string.IsNullOrEmpty(userId)
                                && UserId == tInfoOffre[0]
                                && ((codeOffre != tInfoOffre[1])
                                        || (version != tInfoOffre[2])
                                        || (type != parmPlus[0]) || (numAvenant != parmPlus[1])))
                        || string.IsNullOrEmpty(userId))
                    {

                        model.Add(new ModeleOffreVerrouillee
                        {
                            Utilisateur = tInfoOffre[0],
                            NumOffre = tInfoOffre[1],
                            Version = tInfoOffre[2],
                            Type = parmPlus[0],
                            NumAvenant = Convert.ToInt32(parmPlus[1]),
                            DateVerouillage = AlbConvert.ConvertStrToDate(tInfoOffre[4].ToString(CultureInfo.InvariantCulture))
                        });
                    }
                }
            }
            return model;
        }

        #endregion
    }
}
