using Albingia.Kheops.OP.Application.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Domain.Extensions
{
    public static class EnumExtension
    {
        private static  object syncRoot = new object();
        private static Dictionary<Type, Dictionary <string, string>> names = new Dictionary<Type, Dictionary<string, string>>();
        private static Dictionary<Type, Dictionary <string, object>> values = new Dictionary<Type, Dictionary<string, object>>();
        public static string AsString<T>(this T value) where T : struct {
            var t = typeof(T);
            if (!t.IsEnum) { throw new ArgumentException("Argument is not an Enum"); }
            if (!names.ContainsKey(t))
            {
                lock (syncRoot)
                {
                    if (!names.ContainsKey(t))
                    {
                        var namesValues = GetFields(t).Select(x => new { x.Name, Value = x.GetCustomAttributes(false).OfType<StringValueAttribute>().FirstOrDefault()?.Value ?? x.Name }).ToDictionary(x => x.Name, x => x.Value);
                        names[t] = namesValues;
                    }
                }
            }
            return names[t][value.ToString()];
        }

        public static T AsEnum<T>(this string value) where T : struct {
            var t = typeof(T);
            value = (value??String.Empty).Trim();
            if (!t.IsEnum) { throw new ArgumentException("Argument is not an Enum"); }
            if (!values.ContainsKey(t))
            {
                lock (syncRoot)
                {
                    if (!values.ContainsKey(t))
                    {
                        T sample = Activator.CreateInstance<T>();
                        var valueNames = GetFields(t).Select(x => new { enumValue = x.GetValue(sample), Value = x.GetCustomAttributes(false).OfType<StringValueAttribute>().FirstOrDefault()?.Value ?? x.Name }).ToDictionary(x => x.Value, x => x.enumValue);
                        values[t] = valueNames;
                    }
                }
            }
            return (T) (values[t].ContainsKey(value)  ? values[t][value] : default(T));

        }

        public static IDictionary<string, string> AsRefList<T>() where T : struct
        {
            var t = typeof(T);
            if (!t.IsEnum) { throw new ArgumentException("Argument is not an Enum"); }
            return Enum.GetValues(t).Cast<T>().ToDictionary(x => x.AsString(), x => x.ToString());
        }

        private static IEnumerable<System.Reflection.FieldInfo> GetFields(Type t)
        {
            return t.GetFields().Where(x => (x.Attributes & System.Reflection.FieldAttributes.RTSpecialName) == 0);
        }
    }
}
