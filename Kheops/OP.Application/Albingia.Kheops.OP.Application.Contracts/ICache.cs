using System;

namespace Albingia.Kheops.OP.Application.Contracts {
    public interface IGenericCache
    {
        //T Get<T>(object[] key, Func<object[], string> keyer, Func<object[], T> getter);
        T Get<U, T>(U key, Func<U, string> keyer, Func<U, T> getter);
        T Get<T>(String key, Func<string, T> getter);
        T Get<T>(Func<T> getter);

        void Invalidate<U,T>(U key, Func<U, string> keyer);
        void Invalidate<T>(string key);
    }
}
