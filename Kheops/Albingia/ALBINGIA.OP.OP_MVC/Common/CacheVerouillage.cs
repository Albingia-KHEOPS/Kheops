using Albingia.Kheops.OP.Domain.Affaire;
using ALBINGIA.Framework.Common.CacheTools;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.OP.OP_MVC.Controllers;
using OPServiceContract.IAdministration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
namespace ALBINGIA.OP.OP_MVC.Common
{
    public class CacheVerouillage
    {
        public static List<string> GetAllCache(string splitCaracter, string user = "", string numeroOffre = "", DateTime? dateVerrou = null)
        {
           
            var lstOffreCache = CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.GetAllCache(MvcApplication.SPLIT_CONST_HTML);
         

            if (lstOffreCache == null || !lstOffreCache.Any())
            {
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>())
                {
                    var voletsBlocsCategoriesClient=client.Channel;
                    var datev = dateVerrou.HasValue ? AlbConvert.ConvertDateToInt(dateVerrou).Value.ToString(CultureInfo.CurrentCulture) : null;
                    var lstOffresVerouillees = voletsBlocsCategoriesClient.GetOffresVerouillees(false, false, string.IsNullOrEmpty(numeroOffre) ? string.Empty : numeroOffre.PadLeft(9, ' '), string.Empty, string.Empty, user, datev, datev).ToList();
                    if (lstOffresVerouillees.Count > 0)
                    {
                        lstOffresVerouillees.ForEach(ov => CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.SetIdOffre(ov.Utilisateur, ov.NumOffre
                          , ov.Version.ToString(CultureInfo.CurrentCulture), ov.Type + "_" + ov.NumAvenant, MvcApplication.SPLIT_CONST_HTML));
                        lstOffreCache = CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.GetAllCache(MvcApplication.SPLIT_CONST_HTML);
                    }
                }
            }
            return lstOffreCache;
        }
    }

    public class CommonVerouillage
    {
        public static void DeverrouilleFolder(string codeOffre, string version, string type, string numAvn, string tabGuid, bool isAlimStat, bool isReadonly , bool isModifHorsAvn, bool isAnnul = false)
        {
            FolderController.DeverrouillerAffaire(tabGuid, new AffaireId {
                CodeAffaire = codeOffre,
                IsHisto = false,
                NumeroAliment = int.TryParse(version, out int i) ? i : default,
                TypeAffaire = type.ParseCode<AffaireType>()
            });

            CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.DeleteFromCache(
                AlbSessionHelper.ConnectedUser,
                codeOffre,
                version,
                type + "_" + numAvn,
                MvcApplication.SPLIT_CONST_HTML);
        }

        public static void UnlockFolderByName(string numeroOffre, string tabGuid, bool? isModifHorsAvn = null, bool readOnly = false)
        {
            string guid = string.Empty;
            if (tabGuid.ContainsChars()) {
                guid = tabGuid.Split(new[] { "tabGuid" }, StringSplitOptions.None)[1];
            }
            var elem = numeroOffre.Split(new[] { "||", "_" }, StringSplitOptions.None);
            var key = new FolderKey(elem[0].Trim(), int.Parse(elem[1].Trim()), elem[2].Trim(), int.Parse(elem[3]), AlbSessionHelper.ConnectedUser, guid);
            if (!isModifHorsAvn.HasValue) {
                isModifHorsAvn = AlbSessionHelper.IsModfiHorsAvenant(tabGuid, numeroOffre);
            }
            if (!readOnly || isModifHorsAvn.Value) {
                CacheDictionnaryAccesOffre.CacheInstanceAccesOffre.DeleteFromCache(AlbSessionHelper.ConnectedUser, elem[0].Trim(), elem[1],
                    elem[2] + "_" + elem[3],
                    MvcApplication.SPLIT_CONST_HTML);

                var paramOffre = string.Format("{0}_{1}_{2}_{3}", elem[0].Trim(), elem[1].Trim(), elem[2].Trim(), elem[3]);
                AlbSessionHelper.CurrentFolders.Remove(key);
                using (var client = Framework.Common.ServiceFactory.ServiceClientFactory.GetClient<IVoletsBlocsCategories>()) {
                    var voletsBlocsCategoriesClient=client.Channel;
                    //TODO :[ZBO:09012013:Ajout du type de l'offre comme critère de suppression de l'offre]  
                    voletsBlocsCategoriesClient.SupprimerOffreVerouillee(elem[0].Trim(), elem[1], elem[2], elem[3], AlbSessionHelper.ConnectedUser,
                        string.Empty, false ,isModifHorsAvn.Value, false);
                }
                if (AlbSessionHelper.CurrentFolders == null || AlbSessionHelper.CurrentFolders.Count <= 0) {
                    return;
                }
            }
            if (string.IsNullOrEmpty(guid)) {
                var keys = AlbSessionHelper.CurrentFolders.Keys.Where(k =>
                    k.CodeOffre == key.CodeOffre
                    && k.Version == key.Version
                    && k.Type == key.Type
                    && k.NumeroAvenant == key.NumeroAvenant
                    && k.User == key.User);
                //var folder = AlbSessionHelper.CurrentFolders.Where(el =>
                //    el.Value.Id.Contains(AlbSessionHelper.ConnectedUser + "_" +
                //                         elem[0].Trim() + "_" + elem[1] + "_" +
                //                         elem[2] + "_" + elem[3]));
                keys.ToList().ForEach(k => AlbSessionHelper.CurrentFolders.Remove(k));
            }
            else {
                AlbSessionHelper.CurrentFolders.Remove(key);
            }
        }
    }


}