using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Extensions {
    public static class CollectionExtensions {
        public static bool IsIn<T>(this T? value, params T[] values) where T : struct {
            if (values is null) {
                throw new ArgumentException("Parameters cannot be NULL", nameof(values));
            }
            else if (!value.HasValue) {
                return false;
            }

            return values.Any(v => v.Equals(value.Value));
        }

        public static bool IsIn<T>(this T value, params T[] values) where T : struct {
            return IsIn(new T?(value), values);
        }

        /// <summary>
        /// Defines whether a string value is present in a params list (case insensitive)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsIn(this string value, params string[] values) {
            if (values?.Length < 1) {
                throw new ArgumentException("Parameters cannot be NULL nor empty", nameof(values));
            }

            return values.Any(v => string.Compare(v, value, true) == 0);
        }

        public static void AddRange<T>(this ICollection<T> col, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                col.Add(value);
            }
        }

        public static bool AnyNotNull<T>(this IEnumerable<T> list) {
            if (list is null) {
                return false;
            }
            return list.Any(x => x != null);
        }
    }
}
