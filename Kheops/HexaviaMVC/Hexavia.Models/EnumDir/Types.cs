using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Hexavia.Models.EnumDir
{
    public static class Types
    {
        internal static ConcurrentDictionary<Enum, object> EnumsDataCache = new ConcurrentDictionary<Enum, object>();
        public static string AsCode(this Enum @enum, string defaultCode = null, bool setValueAsDefault = true)
        {
            string code = @enum.AsCode<BusinessCodeAttribute, string>(defaultCode);
            if (code == defaultCode && setValueAsDefault)
            {
                return @enum.ToString();
            }
            return code;
        }

        public static TCode AsCode<TAtt, TCode>(this Enum @enum, TCode defaultCode = default(TCode)) where TAtt : Attribute, ICodeAttribute<TCode>
        {
            return (TCode)EnumsDataCache.GetOrAdd(@enum, (value) => {
                var attribute = value.GetType().GetField(@enum.ToString())?.GetCustomAttribute(typeof(TAtt));
                return attribute == null ? defaultCode : ((TAtt)attribute).Code;
            });
        }
    }
}
