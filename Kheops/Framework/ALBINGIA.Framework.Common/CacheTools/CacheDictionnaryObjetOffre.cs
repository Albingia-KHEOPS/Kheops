using System;

namespace ALBINGIA.Framework.Common.CacheTools
{
    public sealed class CacheDictionnaryObjetOffre : BaseCache
    {
        public static CacheDictionnaryObjetOffre CacheInstanceObjetOffre
        {
            get
            {
                CacheDictionnaryObjetOffre cacheDictionnaryObjetOffre = BaseCacheInstance.CacheGlobalInit<CacheDictionnaryObjetOffre>.CacheInstance;
                string internalNameApp = BaseCacheInstance.CacheGlobalInit<CacheDictionnaryObjetOffre>.External.InternalApplicationId;
                if (string.IsNullOrEmpty(internalNameApp))
                {
                    InternalNameApp = IOFile.FileContentManager.GetConfigValue("InternalApplicationId");
                }
                return cacheDictionnaryObjetOffre;
            }
        }

        /// <summary>
        /// Test si l'offre est cache ou pas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="numOffre"></param>
        /// <param name="version"></param>
        /// <param name="splitCaracter"></param>
        /// <returns></returns>
        public bool IsInCache<T>(string userId, string numOffre, string version, string type, string splitCaracter)
        {
            return ContainKey<T>(InternalNameApp + splitCaracter + CacheKeyConstants.KEYOBJECTOFFRE + splitCaracter + userId + splitCaracter + numOffre + splitCaracter + version + splitCaracter + type);
        }

        /// <summary>
        /// Met à jour la cache (Ajout ou mise à jour)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="offre"></param>
        /// <param name="splitCaracter"></param>
        /// <param name="numOffre"></param>
        /// <param name="version"></param>
        //TODO : Revoir les appels ? 
        public void SetOffre<T>(string userId, T offre, string splitCaracter, string numOffre, string version, string type)
        {
            if (!string.IsNullOrEmpty(numOffre) && !string.IsNullOrEmpty(version))
            {
                var cleUniqueCacheOffre = InternalNameApp + splitCaracter + CacheKeyConstants.KEYOBJECTOFFRE + splitCaracter + userId + splitCaracter + numOffre + splitCaracter + version + splitCaracter + type;
                if (base.IsInCache<T>(cleUniqueCacheOffre))
                {
                    UpdateElemInCache(cleUniqueCacheOffre, offre);
                }
            }
        }
    }

}
