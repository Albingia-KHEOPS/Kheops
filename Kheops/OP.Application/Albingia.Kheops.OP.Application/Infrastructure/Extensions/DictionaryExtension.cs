using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Infrastructure.Extension
{
    public static class DictionaryExtension
    {
        public static T GetValueOrDefault<K,T>(this IDictionary<K,T> dict, K key) {
            var res = default(T);
            if (dict.TryGetValue(key, out res)) {
                return res;
            }
            return default(T);
        }
    }
}
