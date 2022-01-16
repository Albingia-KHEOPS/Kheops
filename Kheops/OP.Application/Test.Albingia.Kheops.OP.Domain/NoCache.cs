using System;
using Albingia.Kheops.OP.Application.Infrastructure.Services;

namespace Test.Albingia.Kheops.OP.Domain
{
    internal class NoCache : ILiveDataCache
    {
        public NoCache()
        {
        }

        public T Get<U, T>(U key, Func<U, string> keyer)
        {
            return default(T);
        }

        public T Get<T>(string key)
        {
            return default(T);

        }

        public T Get<T>()
        {
            return default(T);

        }

        public T Get<U, T>(U key, Func<U, string> keyer, Func<U, T> getter)
        {
            return getter(key);
        }

        public T Get<T>(string key, Func<string, T> getter)
        {
            return getter(key);
        }

        public T Get<T>(Func<T> getter)
        {
            return getter();
        }

        public void Invalidate<U, T>(U key, Func<U, string> keyer)
        {
            
        }

        public void Invalidate<T>(string key)
        {
           
        }

        public void Set<U, T>(U key, Func<U, string> keyer, T value)
        {
            
        }

        public void Set<T>(string key, T value)
        {
            
        }
    }
}