using System;
using System.Runtime.Caching;

namespace Albingia.Kheops.OP.Application.Contracts {
    public interface ILiveDataCache : IGenericCache
    {
        T Get<U, T>(U key, Func<U, string> keyer);
        T Get<T>(String key);
        T Get<T>();

        void Set<U, T>(U key, Func<U, string> keyer, T value);
        void Set<T>(string key, T value);

        void SetPolicy<T>(Func<CacheItemPolicy> behavior);
    }
}