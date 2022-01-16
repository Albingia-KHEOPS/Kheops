using System;
using System.Globalization;
using System.Text;


namespace ALBINGIA.Framework.Common.CacheTools
{
    public class CacheDictionnaryFormuleGaranties:BaseCache
    {
        #region Creation Instance
        public static CacheDictionnaryFormuleGaranties CacheInstanceFormuleGaranties
        {
            get
            {
                var cacheDictionnaryObjetOffre = BaseCacheInstance.CacheGlobalInit<CacheDictionnaryFormuleGaranties>.CacheInstance;
                var internalNameApp = BaseCacheInstance.CacheGlobalInit<CacheDictionnaryFormuleGaranties>.External.InternalApplicationId;
                if (string.IsNullOrEmpty(internalNameApp))
                {
                    InternalNameApp = IOFile.FileContentManager.GetConfigValue("InternalApplicationId");
                }
                return cacheDictionnaryObjetOffre;
            }
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="offre"></param>
        /// <param name="splitCaracter"></param>
        /// <param name="numOffre"></param>
        /// <param name="numVersion"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <returns></returns>
        public void DeleteFromCache<T>(string userId, T offre, string splitCaracter, string numOffre, string numVersion, string type, int codeFormule, int codeOption)
        {
            var paramCacheFormule = ToKeyCache(userId, splitCaracter, numOffre, numVersion, type, codeFormule, codeOption);
            RemoveFromCache(paramCacheFormule.ToString());
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="formuleGarantie"></param>
        /// <param name="splitCaracter"></param>
        /// <param name="numOffre"></param>
        /// <param name="numVersion"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <param name="modeleFormuleGarantie"></param>
        public void SetFormuleGarantieToCache<T>(string userId, string splitCaracter, string numOffre, string numVersion, string type, int codeFormule, int codeOption,T modeleFormuleGarantie)
        {
            var paramCacheFormule = ToKeyCache(userId, splitCaracter, numOffre, numVersion, type, codeFormule, codeOption);
            if (!IsInCache<T>(paramCacheFormule.ToString()))
            {
                UpdateElemInCache(paramCacheFormule.ToString(), modeleFormuleGarantie);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="formuleGarantie"></param>
        /// <param name="splitCaracter"></param>
        /// <param name="numOffre"></param>
        /// <param name="numVersion"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <returns></returns>
        public T GetFormuleGarantieCache<T>(string userId, string splitCaracter, string numOffre, string numVersion, string type, int codeFormule, int codeOption) where T : class
        {
            var paramCacheFormule = ToKeyCache(userId, splitCaracter, numOffre, numVersion, type, codeFormule, codeOption);
            if (!IsInCache<T>(paramCacheFormule.ToString()))
                return null;

            return (base[paramCacheFormule.ToString()] as T);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="splitCaracter"></param>
        /// <param name="numOffre"></param>
        /// <param name="numVersion"></param>
        /// <param name="type"></param>
        /// <param name="codeFormule"></param>
        /// <param name="codeOption"></param>
        /// <returns></returns>
        //public bool IsInCache<T>(string userId, string splitCaracter, string numOffre, string numVersion, string type, int codeFormule, int codeOption=0) where T : class
        public bool IsInCache<T>(string userId, string splitCaracter, string numOffre, string numVersion, string type, int codeFormule, int codeOption) where T : class
        {
            var paramCacheFormule = ToKeyCache(userId, splitCaracter, numOffre, numVersion, type, codeFormule, codeOption);
            return ContainKey<T>(paramCacheFormule.ToString());
        }
        
        /// <summary>
        /// Supprime toutes les formules de garanties du cache
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="splitCaracter"></param>
        /// <param name="numOffre"></param>
        /// <param name="numVersion"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool DeleteAllFormuleCache(string userId, string splitCaracter, string numOffre, string numVersion, string type)
        {
            var paramCacheFormule = new StringBuilder();
            paramCacheFormule.Append(InternalNameApp + splitCaracter + CacheKeyConstants.KEYFORMULEGAR + splitCaracter);
            paramCacheFormule.Append(userId + splitCaracter + numOffre + splitCaracter);
            paramCacheFormule.Append(numVersion + splitCaracter + type);
            var lstFormulesCache = GetAllKeysItems(paramCacheFormule.ToString());
            if (lstFormulesCache!=null)
                GetAllKeysItems(paramCacheFormule.ToString()).ForEach(el => RemoveFromCache(el));
            return true;

        }
        #endregion
        #region Méthodes privées
        private static StringBuilder ToKeyCache(string userId, string splitCaracter, string numOffre, string numVersion,
                                                   string type, int codeFormule, int codeOption)
        {
            var paramCacheFormule = new StringBuilder();
            paramCacheFormule.Append(InternalNameApp + splitCaracter + CacheKeyConstants.KEYFORMULEGAR + splitCaracter);
            paramCacheFormule.Append(userId + splitCaracter + numOffre + splitCaracter);
            paramCacheFormule.Append(numVersion + splitCaracter + type + splitCaracter);
            paramCacheFormule.Append(codeFormule.ToString(CultureInfo.CurrentCulture) + splitCaracter + codeOption);
            return paramCacheFormule;
        }
        #endregion

       
    }
}
