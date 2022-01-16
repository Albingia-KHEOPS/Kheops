using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Albingia.Kheops.OP.Application.Infrastructure.Tools {

    public class SharedCache : LiveCache, ISharedDataCache
    {
        public SharedCache(ObjectCache cache, ISessionContext context) : base(cache, context) { }

    }

    public class LiveCache : ILiveDataCache
    {
        private ObjectCache cache;
        private ISessionContext context;
        private ConcurrentDictionary<Type, Func<CacheItemPolicy>> policies = new ConcurrentDictionary<Type, Func<CacheItemPolicy>>();
        private Func<CacheItemPolicy> defaultPolicy;

        private LiveCache()
        {
            defaultPolicy = () => {
                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = TimeSpan.FromMinutes(this.context.Timeout);
                return policy;
            };
        }
        private static DateTimeOffset GetNextExpiration(DateTimeOffset date, int roudingMinutes)
        {
            date = new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0, date.Offset).AddSeconds(30);
            return date.AddMinutes(roudingMinutes - ((date.Minute + date.Hour * 60) % roudingMinutes));
        }

        public LiveCache(ObjectCache cache, ISessionContext context) : this()
        {
            this.cache = cache;
            this.context = context;
        }


        private CacheItemPolicy GetPolicy<T>()
        {
            Func<CacheItemPolicy> policy;
            if (!policies.TryGetValue(typeof(T), out policy)) {
                policy = defaultPolicy;
            };
            return policy();
        }

        public IEnumerable<string> GetKeys<T>()
        {
            var t = typeof(T).FullName;
            return this.cache.Select(x => x.Key).Where(x => x.StartsWith(t + "/"));
        }
        public T Get<U, T>(U key, Func<U, string> keyer, Func<U, T> getter)
        {
            string composedKey = keyer(key);
            return this.Get(composedKey, x => getter(key));
        }
        public T Get<T>(string key, Func<string, T> getter)
        {
            string composedKey = MakeKey<T>(key);
            var val = this.cache[composedKey];
            T result;

            if (val is null) {
                result = getter(key);
                this.cache.Add(composedKey, result, GetPolicy<T>());
            } else {
                result = (T)val;
            }

            return result;
        }

        private string MakeKey<T>(string key)
        {
            var sessionId = this.context.SessionId;
            var t = typeof(T).FullName;
            var composedKey = t + "/" + sessionId + "/" + key;
            return composedKey;
        }

        public void Invalidate<U, T>(U key, Func<U, string> keyer)
        {
            this.Invalidate<T>(keyer(key));
        }
        public void Invalidate<T>(string key)
        {
            this.cache.Remove(MakeKey<T>(key));
        }


        public T Get<T>(Func<T> getter)
        {
            return this.Get("/ALL/", x => getter());
        }

        public void Set<U, T>(U key, Func<U, string> keyer, T value)
        {
            this.Set(keyer(key), value);
        }

        public void Set<T>(string key, T value)
        {
            this.cache.Set(MakeKey<T>(key), value, GetPolicy<T>());
        }

        public void SetPolicy<T>(Func<CacheItemPolicy> behavior) {
            if (behavior !=null && !this.policies.TryGetValue(typeof(T), out var f)) {
                this.policies.TryAdd(typeof(T), behavior);
            }
        }

        public T Get<U, T>(U key, Func<U, string> keyer)
        {
            string composedKey = keyer(key);
            return Get<T>(composedKey);
        }

        public T Get<T>(string key)
        {
            string composedKey = MakeKey<T>(key);
            var val = this.cache[composedKey];
            T result;

            if (val is null) {
                return default(T);
            } else {
                result = (T)val;
            }

            return result;
        }

        public T Get<T>()
        {
            return this.Get<T>("/ALL/");
        }
    }

    internal class SessionChangeMonitor : ChangeMonitor
    {
        private string id;

        public SessionChangeMonitor(string id)
        {
            this.id = id;
        }
        protected override void Dispose(bool disposing)
        {

        }
        public override string UniqueId => id;
    }

    //public class LiveCache: Services.ILiveDataCache {
    //    public T Get<U, T>(U key, Func<U, string> keyer, Func<U, T> getter) {
    //        throw new NotImplementedException();
    //    }

    //    public T Get<T>(string key, Func<string, T> getter) {
    //        throw new NotImplementedException();
    //    }

    //    public T Get<T>(Func<T> getter) {
    //        throw new NotImplementedException();
    //    }

    //    public void Invalidate<U, T>(U key, Func<U, string> keyer) {
    //        throw new NotImplementedException();
    //    }

    //    public void Set<U, T>(U key, Func<U, string> keyer, T value) {
    //        throw new NotImplementedException();
    //    }

    //    public void Set<T>(string key, T value) {
    //        throw new NotImplementedException();
    //    }
    //}
}
