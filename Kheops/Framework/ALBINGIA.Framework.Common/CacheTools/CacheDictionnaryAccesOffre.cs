using System;
using System.Collections.Generic;

namespace ALBINGIA.Framework.Common.CacheTools
{
    public sealed class CacheDictionnaryAccesOffre : BaseCache
    {

        #region Propriétés
        /// <summary>
        /// 
        /// </summary>
        public static CacheDictionnaryAccesOffre CacheInstanceAccesOffre
        {
            get
            {

                CacheDictionnaryAccesOffre cacheDictionnaryAccesOffre = BaseCacheInstance.CacheGlobalInit<CacheDictionnaryAccesOffre>.CacheInstance;
                string internalNameApp = BaseCacheInstance.CacheGlobalInit<CacheDictionnaryAccesOffre>.External.InternalApplicationId;
                if (string.IsNullOrEmpty(internalNameApp))
                {
                    InternalNameApp = IOFile.FileContentManager.GetConfigValue("InternalApplicationId");
                }
                return cacheDictionnaryAccesOffre;
            }
        }


        #endregion

        #region Méthodes publiques


        /// <summary>
        /// Supprime tousle cache offre
        /// </summary>
        /// <param name="splitCaracter"></param>
        /// <returns></returns>
        public void RemoveCacheOffre(string splitCaracter)
        {
            DeleteAllWithKey(InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="numeroOffre"></param>
        /// <param name="version"></param>
        /// <param name="typeOffre"></param>
        /// <param name="splitCaracter"> </param>
        /// <returns></returns>
        public void DeleteFromCache(string userId, string numeroOffre, string version, string typeOffre, string splitCaracter)
        {

            var lst = GetAllKeysItems(InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE);
            var uniqueKeyCache = lst == null ? string.Empty : lst.Find(str => str.Contains(numeroOffre.Trim() + splitCaracter + version + splitCaracter + typeOffre));
            DeleteAllWithKey(uniqueKeyCache);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="splitCaracter"> </param>
        /// <param name="connectedUser"></param>
        /// <param name="numeroOffre"></param>
        /// <param name="dateVerrou"></param>
        /// <returns></returns>
        public List<string> GetAllCache(string splitCaracter, string connectedUser = "", string numeroOffre = "", DateTime? dateVerrou = new DateTime?())
        {
            numeroOffre = numeroOffre.Trim();
            string user = GetUser(connectedUser);
            var keyCache = InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE;
            var listAllCache = GetAllItems(keyCache);
            if (!(!string.IsNullOrEmpty(user) || !string.IsNullOrEmpty(numeroOffre) || dateVerrou.HasValue))
            {
                return listAllCache;
            }
            var retlstCache = listAllCache;
            if (!string.IsNullOrEmpty(numeroOffre))
            {
                string offreToFind;
                string[] elemOffre = numeroOffre.Split(new[] { '_' });
                if (elemOffre.Length == 2)
                {
                    offreToFind = elemOffre[0].Trim() + splitCaracter + elemOffre[1];
                }
                else
                {
                    offreToFind = elemOffre[0].Trim();
                }
                retlstCache = listAllCache.FindAll(m => m.Contains(offreToFind));
            }
            if (!string.IsNullOrEmpty(user))
            {
                retlstCache = listAllCache.FindAll(m => m.Contains(user));
            }
            return retlstCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="offreId"></param>
        /// <param name="offreVersion"></param>
        /// <param name="typeOffre"></param>
        /// <param name="splitCaracter"> </param>
        /// <returns></returns>
        public string GetOffreCache(string userId, string offreId, string offreVersion, string typeOffre, string splitCaracter)
        {
            userId = GetUser(userId);
            offreId = offreId.Trim();
            var cleUniqueCacheOffre = GetUniqueKeyCache(userId, offreId, offreVersion, typeOffre, splitCaracter);
            if (!IsInCache<string>(cleUniqueCacheOffre))
            {
                // string numeroOffre = offreId + "_" + offreVersion;
                // base.AddToCache<string>(cleUniqueCacheOffre, numeroOffre, null);

                SetIdOffre(userId, offreId, offreVersion, typeOffre, splitCaracter);
            }
            return (base[cleUniqueCacheOffre] as string);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="numOffre"></param>
        /// <param name="version"></param>
        /// <param name="typeOffre"></param>
        /// <param name="splitCaracter"> </param>
        /// <param name="user"> </param>
        /// <param name="isInCache"></param>
        /// <returns></returns>
        public bool IsInCache(string userId, string numOffre, string version, string typeOffre, string splitCaracter, out string user, out bool isInCache)
        {
            user = string.Empty;
            isInCache = false;

            var keyCache = GetOfferFromCache(splitCaracter + numOffre.Trim() + splitCaracter + version + splitCaracter + typeOffre, InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE);
            if (string.IsNullOrEmpty(keyCache))
                return false;

            var vLstKeyCache = keyCache.Split(new[] { splitCaracter }, StringSplitOptions.None);
            user = GetUser(vLstKeyCache[2]);
            isInCache = vLstKeyCache[0] + splitCaracter + vLstKeyCache[1] == InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE;
            return ((!string.IsNullOrEmpty(keyCache)
                     &&
                     (vLstKeyCache[0] + splitCaracter + vLstKeyCache[1] ==
                      InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE)));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="numeroOffre"></param>
        /// <param name="version"></param>
        /// <param name="typeOffre"></param>
        /// <param name="splitCaracter"></param>
        /// <returns></returns>
        public void SetIdOffre(string userId, string numeroOffre, string version, string typeOffre, string splitCaracter)
        {
            numeroOffre = numeroOffre.Trim();
            userId = GetUser(userId);
            //TODO : achanger avec l'objet offre vérouillé
            if (!string.IsNullOrEmpty(numeroOffre) && !string.IsNullOrEmpty(version))
            {
                var cleUniqueCacheOffre = InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE + splitCaracter + userId + splitCaracter + numeroOffre + splitCaracter + version + splitCaracter + typeOffre;
                if (!IsInCache<string>(cleUniqueCacheOffre))
                {
                    UpdateElemInCache(cleUniqueCacheOffre, userId + splitCaracter + numeroOffre + splitCaracter + version + splitCaracter + typeOffre + splitCaracter + DateTime.Now.ToString("dd/MM/yyyy HH:mm tt"));
                }
            }

        }

        #endregion

        #region Méthodes privées
        private static string GetUniqueKeyCache(string userId, string offreId, string offreVersion, string typeOffre, string splitCaracter)
        {
            return InternalNameApp + splitCaracter + CacheKeyConstants.KEYACCESOFFRE + splitCaracter + userId + splitCaracter + offreId + splitCaracter + offreVersion + splitCaracter + typeOffre;

        }
        private static string GetUser(string user)
        {
            return user.Contains("\\") ? user.Split(new[] { "\\" }, StringSplitOptions.None)[1].ToUpper() : user.ToUpper();
        }
        #endregion
    }

}
