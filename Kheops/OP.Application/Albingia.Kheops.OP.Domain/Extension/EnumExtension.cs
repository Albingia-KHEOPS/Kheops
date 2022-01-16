using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Albingia.Kheops.OP.Domain.Extension
{
    public static class EnumExtension
    {
        private static ConcurrentDictionary<Type, Dictionary<string, string>> names = new ConcurrentDictionary<Type, Dictionary<string, string>>();
        private static ConcurrentDictionary<Type, Dictionary<string, object>> values = new ConcurrentDictionary<Type, Dictionary<string, object>>();

        public static string AsString(this Enum value)
            => names.GetOrAdd(value.GetType(), GetStringForEnum)[value.ToString()];

        public static T AsEnum<T>(this string value) where T : struct
        {
            Type type = typeof(T);
            value = (value ?? string.Empty).Trim();
            if (!type.IsEnum) { throw new ArgumentException("Argument is not an Enum"); }
            values.TryAdd(type, GetEnumForString(type));
            return (T)(values[type].ContainsKey(value) ? values[type][value] : default(T));
        }

        public static IDictionary<string, string> AsRefList<T>() where T : struct
        {
            var t = typeof(T);
            if (!t.IsEnum) { throw new ArgumentException("Argument is not an Enum"); }
            return Enum.GetValues(t).Cast<Enum>().ToDictionary(x => x.AsString(), x => x.ToString());
        }

        private static IEnumerable<FieldInfo> GetFields(Type type)
            => type.GetFields().Where(x => (x.Attributes & FieldAttributes.RTSpecialName) == 0);

        private static Dictionary<string, string> GetStringForEnum(Type type)
            => GetFields(type)
                .Select(
                    x => new { x.Name, Value = GetStringValueAttribute(x) ?? x.Name }
                ).ToDictionary(x => x.Name, x => x.Value);

        private static Dictionary<string, object> GetEnumForString(Type type)
            => GetFields(type)
                .Select(
                    x => new { EnumValue = x.GetValue(Activator.CreateInstance(type)), Value = GetStringValueAttribute(x) ?? x.Name }
                ).ToDictionary(x => x.Value, x => x.EnumValue);

        private static string GetStringValueAttribute(FieldInfo x)
            => x.GetCustomAttributes(false).OfType<BusinessCodeAttribute>().FirstOrDefault()?.Code
            ?? x.GetCustomAttributes(false).OfType<AlbEnumInfoValue>().FirstOrDefault()?.Value;
    }
}
