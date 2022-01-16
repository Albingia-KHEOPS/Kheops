using System;
using System.Collections.Generic;
using System.Linq;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.Framework.Common
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AlbEnumInfoValue : Attribute, ICodeAttribute<string>
    {
        private readonly string _value;

        public AlbEnumInfoValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
        public string Code => Value; 

        public static string GetEnumInfoSplit(Enum value, out string text)
        {
            text = string.Empty;
            var output = string.Empty;

            output = GetEnumInfo(value);

            if (!string.IsNullOrEmpty(output))
            {
                text = output.Split(new[] { "_" }, StringSplitOptions.None)[1];
                return output.Split(new[] { "_" }, StringSplitOptions.None)[0];
            }

            return string.Empty;
        }

        public static string GetEnumInfo(Enum value)
        {
            var output = string.Empty;
            var type = value.GetType();
            var fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(AlbEnumInfoValue), false).Cast<AlbEnumInfoValue>().ToArray();
            if (attrs.Length > 0) output = attrs[0].Value;

            return output;
        }

        public static T GetEnumValue<T>(string attribut)
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var type = item.GetType();
                if (GetEnumInfo((Enum)item) == attribut)
                    return (T)item;
            }
            return default(T);
        }

        public static List<AlbSelectListItem> GetListEnumInfo<T>()
        {
            List<AlbSelectListItem> list = new List<AlbSelectListItem>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                string text = string.Empty;
                string value = string.Empty;
                value = GetEnumInfoSplit((Enum)item, out text);
                list.Add(new AlbSelectListItem { Text = text, Value = value, Descriptif = string.Format("{0} - {1}", value, text), Selected = false, Title = string.Format("{0} - {1}", value, text) });
            }
            return list;
        }

        public static AlbSelectListItem GetSelectListItemEnumInfo<T>(T item)
        {
            string text = string.Empty;
            string value = string.Empty;
            value = GetEnumInfoSplit(item as Enum, out text);

            return new AlbSelectListItem {  Text = text, Value = value,
                                            Descriptif = string.Format("{0} - {1}", 
                                            value, text), Selected = false,
                                            Title = string.Format("{0} - {1}", value, text) };
       }

    }
}
