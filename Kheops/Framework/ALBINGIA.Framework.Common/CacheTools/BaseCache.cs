using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.InteropServices.WindowsRuntime;
using ALBINGIA.Framework.Common.AlbingiaExceptions;

namespace ALBINGIA.Framework.Common.CacheTools
{


    public class BaseCache
    {

        internal static string InternalNameApp { get; set; }
        // Fields
        private static ObjectCache _cache = MemoryCache.Default;
        private string prefix = "";

        public CacheItemPolicy DefaultPolicy { get; }

        protected BaseCache()
        {
            this.prefix = this.GetType().FullName.Split(new char[] { ',' }).First().Replace(".", "_");
        }
        protected BaseCache(string prefix)
        {
            this.prefix = prefix;
            this.DefaultPolicy = new CacheItemPolicy() { AbsoluteExpiration = new DateTimeOffset().Date.AddDays(1).AddMinutes(-1) };
        }

        // Methods
        protected void AddToCache<T>(string cacheKey, T value)
        {
            this.AddToCache(cacheKey, value, this.DefaultPolicy);
        }
        protected void AddToCache<T>(string cacheKey, T value, CacheItemPolicy policy)
        {
            _cache.Set(Prefix(cacheKey), value, policy);
        }

        protected object this[string key] =>  _cache.Get(Prefix(key));

        protected string Prefix(string key) { return prefix + key; }

        protected bool ContainKey<T>(string cKey)
        {
            return _cache.Contains(Prefix(cKey));
        }

        public List<string> GetAllKeysItems(string cacheKeyConstant)
        {
            var returnList = new List<string>();
            returnList = _cache.Where(m => m.Key.StartsWith(prefix) && m.Key.Contains(cacheKeyConstant)).Select(x=>x.Key.Substring(this.prefix.Length)).ToList();
            return returnList;
        }

        public List<string> GetAllItems(string cacheKeyConstant)
        {
            var returnList = new List<string>();
            returnList = _cache.Where(m => m.Key.StartsWith(prefix) && m.Key.Contains(cacheKeyConstant)).Select(x => (string)x.Value).ToList();
            return returnList;
        }

        protected string GetOfferFromCache(string substrKey, string strKey = "")
        {
            try
            {
                bool hasKey = !string.IsNullOrEmpty(strKey);
                KeyValuePair<string, object> elemCache = _cache.FirstOrDefault(m => m.Key.StartsWith(prefix) && m.Key.Contains(substrKey) && (!hasKey || m.Key.Contains(strKey)));

                return (!string.IsNullOrEmpty(elemCache.Key) ? elemCache.Key.Substring(this.prefix.Length) : string.Empty);
            }
            catch (Exception ex)
            {
                new AlbTechException(ex);
                return string.Empty;
            }

        }

        protected bool IsInCache<T>(string cKey)
        {
            var pKey = Prefix(cKey);
            return _cache.Contains(Prefix(pKey)) && _cache.Get(pKey) is T;
        }

        protected void RemoveFromCache(string cKey)
        {
            var pKey = Prefix(cKey);
            if (_cache.Contains(pKey))
            {
                _cache.Remove(pKey);
            }
        }

        protected void DeleteAllWithKey(string cKey)
        {
            if (string.IsNullOrEmpty(cKey))
            {
                return;
            }
            var pKey = Prefix(cKey);

            _cache.Where(x => x.Key.StartsWith(prefix) && x.Key.Contains(cKey)).ToList().ForEach(x=>_cache.Remove(x.Key));
            
        }

        protected void UpdateElemInCache<T>(string cacheKey, T value)
        {
            this.AddToCache(cacheKey, value);
        }
    }



}
