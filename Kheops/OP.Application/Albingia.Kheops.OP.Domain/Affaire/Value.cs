using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albingia.Kheops.OP.Domain.Affaire
{
    public class Value
    {
        private static Dictionary<Type, Func<object,object>[]> GettersCache = new Dictionary<Type, Func<object, object>[]>();
        private static Func<object, object>[] GetGetters(Type t) {
             if (!GettersCache.ContainsKey(t)) {
                var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (props.Any()) {
                    GettersCache[t] = props.Select<PropertyInfo, Func<object, object>>(x => ((y) => y is null ? null : x?.GetValue(y))).ToArray();
                }
            }
            return GettersCache[t];
        }

        public override bool Equals(object obj)
        {
            var t = this.GetType();
            var ret = !(obj is null) && obj.GetType() == t;
            ret = ret && GetGetters(t).All( get=>
            {
                var a = get(this);
                var b = get(obj);
                return (a == null && b == null) || (a != null && b != null) && (a.Equals(b));
            });
            return ret;
        }

        public override int GetHashCode() {
            var t = this.GetType();
            return GetGetters(t).Aggregate(17, (hash, get) => {
                unchecked {
                    int hashval = get(this)?.GetHashCode() ?? 0;
                    return hash + 23 * hashval;
                }
            });
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}