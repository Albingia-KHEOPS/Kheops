using Albingia.Kheops.OP.Application.Contracts;
using Albingia.Kheops.OP.Application.Infrastructure;
using Albingia.Kheops.OP.Application.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Albingia.Kheops.OP.Infrastructure
{
    public class Cache : IGenericCache
    {
        private ObjectCache cache;
        private Dictionary<Type, Func<CacheItemPolicy>> policies = new Dictionary<Type, Func<CacheItemPolicy>>();
        private Func<CacheItemPolicy> defaultPolicy = () => new CacheItemPolicy() {
            AbsoluteExpiration = GetNextExpiration(DateTimeOffset.Now, 10)
        };

        private static DateTimeOffset GetNextExpiration(DateTimeOffset date, int roudingMinutes)
        {
            date = new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0, 0, date.Offset).AddSeconds(30);
            return date.AddMinutes(roudingMinutes - ((date.Minute + date.Hour * 60) % roudingMinutes));
        }

        public Cache(ObjectCache cache)
        {
            this.cache = cache;
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
            var k = keyer(key);
            string composedKey = MakeKey<T>(k);
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

        private static string MakeKey<T>(string key)
        {
            var t = typeof(T).FullName;
            var composedKey = t + "/" + key;
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

        public T Get<T>(string key, Func<string, T> getter)
        {
            return this.Get(key, x => x, getter);
        }

        public T Get<T>(Func<T> getter)
        {
            return this.Get("/ALL/", x => getter());
        }


    }

}
