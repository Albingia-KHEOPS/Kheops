using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Extensions {
    public static class Numerics {

        public static readonly IEnumerable<Type> IntegerTypes = new[] {
            typeof(sbyte), typeof(byte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong)
        };

        public static int? ParseInt(this string intString, int? @default = default(int)) {
            return int.TryParse(intString, out int x) ? x : @default;
        }

        public static int? ToInteger(this object o, int? defaultInteger = 0) {
            if (o is null || o == DBNull.Value) {
                return defaultInteger;
            }
            if (o is int) { return (int)o; }
            if (o is string s) {
                return int.TryParse(s, out int i) ? i : defaultInteger;
            }
            if (IntegerTypes.Contains(o.GetType())) {
                try {
                    return Convert.ToInt32(o);
                }
                catch (OverflowException) {
                    return defaultInteger;
                }
            }

            try {
                return Convert.ToInt32(o);
            }
            catch (FormatException) {
                return defaultInteger;
            }
            catch (InvalidCastException) {
                return null;
            }
        }

        public static bool IsBetween(this int integer, int min = int.MinValue, int max = int.MaxValue, bool excludeMin = false, bool excludeMax = false) {
            if (excludeMax && max > int.MinValue) {
                max--;
            }
            if (excludeMin && min < int.MaxValue) {
                min++;
            }
            return integer >= min && integer <= max;
        }
    }
}
