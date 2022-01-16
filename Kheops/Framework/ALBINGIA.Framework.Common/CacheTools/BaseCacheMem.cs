using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace ALBINGIA.Framework.Common.CacheTools
{
    public class BaseCacheMem<T>
    {

        internal static string InternalNameApp { get; set; }
        public List<MemoryCache> CacheFolder { get; set; }

        #region Méthodes Publiques
        protected void AddToCache(string cacheKey, T value, Func<bool> valueCache = null)
        {
            MemoryCache _cache = MemoryCache.Default;

            if (CacheFolder == null) {
                CacheFolder = new List<MemoryCache>();
            }

            var expireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 23, 58, 01,
                DateTimeKind.Local);
            _cache.Set(cacheKey, value, expireDate);
            CacheFolder.Add(_cache);
        }
        protected bool ContainKey(string cKey, Func<bool> value = null)
        {
            if (CacheFolder == null || !CacheFolder.Any()) {
                return false;
            }

            return CacheFolder.Any(elm => elm.Contains(cKey));

        }

        #endregion


    }
}
