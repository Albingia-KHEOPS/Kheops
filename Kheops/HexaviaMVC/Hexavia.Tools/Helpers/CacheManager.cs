using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;

namespace Hexavia.Tools.Helpers
{
    public static class CacheManager
    {
        private static readonly MemoryCache MemoryCache = new MemoryCache("Hexavia");
        private static readonly string CacheConfigPrefix = "Cache.";

        public static void Add(string key, object data, DateTimeOffset timeOffset)
        {
            MemoryCache.Set(key, data, timeOffset);
        }

        public static void Add(string key, object data, DateTimeOffset timeOffset, CacheEntryUpdateCallback updateCallback)
        {
            MemoryCache.Set(key, data, GetCacheItemPolicy(timeOffset, updateCallback));
        }

        public static CacheItemPolicy GetCacheItemPolicy(DateTimeOffset timeOffset, CacheEntryUpdateCallback updateCallback)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                UpdateCallback = updateCallback,
                AbsoluteExpiration = timeOffset,
                Priority = CacheItemPriority.Default,
            };

            return cacheItemPolicy;
        }

        public static bool TryGet<T>(string key, out T value)
        {
            var cachedObject = MemoryCache.Get(key);
            if (cachedObject == null)
            {
                value = default(T);
                return false;
            }

            value = (T)cachedObject;
            return true;
        }

        public static bool TryGet(string key)
        {
            var cachedObject = MemoryCache.Get(key);
            if (cachedObject == null)
            {
                return false;
            }
            return true;
        }


        public static void ClearAll()
        {
            var items = MemoryCache.Select(pair => pair.Key).ToList();
            items.ForEach(key => MemoryCache.Remove(key));
        }

        /// <summary>
        /// Clear cache by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if the cache is removed, false if the cache is not found</returns>
        public static bool ClearCacheByKey(string key)
        {
            if (MemoryCache.Contains(key))
            {
                MemoryCache.Remove(key);
                return true;
            }
            return false;
        }

        public static DateTimeOffset GetAbsoluteExpirationConfiguration(string key)
        {
            var defaultConfig = DateTime.Today.AddDays(1);
            var configKey = string.Concat(CacheConfigPrefix, key);
            var value = ConfigurationManager.AppSettings[configKey];

            if (value == null)
            {
                return defaultConfig;
            }

            TimeSpan configDate;
            if (!TimeSpan.TryParse(value, out configDate))
            {
                return defaultConfig;
            }

            var date = DateTime.Today.Add(configDate);

            return date;
        }

        public static IList<string> GetAllKeys()
        {
            return MemoryCache.Select(item => item.Key).ToList();
        }
    }
}
