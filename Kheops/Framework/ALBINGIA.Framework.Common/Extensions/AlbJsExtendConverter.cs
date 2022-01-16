using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Globalization;

namespace ALBINGIA.Framework.Common.Extensions
{
    public class AlbJsExtendConverter<T> : JavaScriptConverter where T : new()
    {

        private const string _dateFormat = "dd/MM/yyyy";

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new[] { typeof(T) };
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            T p = new T();

            var props = typeof(T).GetProperties();

            foreach (string key in dictionary.Keys)
            {
                var prop = props.FirstOrDefault(t => t.Name == key);
                if (prop != null)
                {
                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        if (string.IsNullOrEmpty(dictionary[key] as string) && prop.PropertyType == typeof(DateTime?))
                            prop.SetValue(p, null, null);
                        else
                            prop.SetValue(p, DateTime.ParseExact(dictionary[key] as string, _dateFormat, DateTimeFormatInfo.InvariantInfo), null);
                    }
                    else
                    {
                        if (prop.PropertyType == typeof(TimeSpan) || prop.PropertyType == typeof(TimeSpan?))
                        {
                            if (string.IsNullOrEmpty(dictionary[key] as string) && prop.PropertyType == typeof(TimeSpan?))
                                prop.SetValue(p, null, null);
                            else
                                prop.SetValue(p, TimeSpan.Parse(dictionary[key] as string), null);

                        }

                        else
                        {
                            if (prop.PropertyType == typeof(Boolean) || prop.PropertyType == typeof(Boolean?)) {
                                prop.SetValue(p, !string.IsNullOrEmpty(dictionary[key].ToString()) && bool.Parse(dictionary[key].ToString()), null);
                            }
                            else {
                                if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?)) {
                                    if (string.IsNullOrEmpty(dictionary[key].ToString())) {
                                        if (prop.PropertyType == typeof(int?))
                                            prop.SetValue(p, null, null);
                                        else
                                            prop.SetValue(p, 0, null);
                                    }
                                    else {
                                        int val = 0;
                                        if (int.TryParse(dictionary[key].ToString(), out val))
                                            prop.SetValue(p, val, null);
                                        else
                                            prop.SetValue(p, null, null);
                                    }

                                    //prop.SetValue(p, !string.IsNullOrEmpty(dictionary[key].ToString()) && int.Parse(dictionary[key].ToString()), null);
                                }
                                else {
                                    if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?)) {
                                        if (string.IsNullOrEmpty(dictionary[key].ToString())) {
                                            if (prop.PropertyType == typeof(decimal?))
                                                prop.SetValue(p, null, null);
                                            else
                                                prop.SetValue(p, 0M, null);
                                        }
                                        else {
                                            decimal val = 0;
                                            if (decimal.TryParse(dictionary[key].ToString(), out val))
                                                prop.SetValue(p, val, null);
                                            else
                                                prop.SetValue(p, null, null);
                                        }
                                    }
                                    else {

                                        if (prop.PropertyType == typeof(Single) || prop.PropertyType == typeof(Single?)) {
                                            if (string.IsNullOrEmpty(dictionary[key].ToString())) {
                                                if (prop.PropertyType == typeof(Single?))
                                                    prop.SetValue(p, null, null);
                                                else
                                                    prop.SetValue(p, 0, null);
                                            }
                                            else {
                                                Single val = 0;
                                                if (Single.TryParse(dictionary[key].ToString(), out val))
                                                    prop.SetValue(p, val, null);
                                                else
                                                    prop.SetValue(p, null, null);
                                            }
                                        }
                                        else if (prop.PropertyType == typeof(Int64) || prop.PropertyType == typeof(Int64?)) {
                                            if (string.IsNullOrEmpty(dictionary[key].ToString())) {
                                                if (prop.PropertyType == typeof(Int64?))
                                                    prop.SetValue(p, null, null);
                                                else
                                                    prop.SetValue(p, 0, null);
                                            }
                                            else {
                                                Int64 val = 0;
                                                if (Int64.TryParse(dictionary[key].ToString(), out val))
                                                    prop.SetValue(p, val, null);
                                                else
                                                    prop.SetValue(p, null, null);
                                            }
                                        }
                                        else if (prop.PropertyType == typeof(Double) || prop.PropertyType == typeof(Double?)) {
                                            if (string.IsNullOrEmpty(dictionary[key].ToString())) {
                                                if (prop.PropertyType == typeof(Double?))
                                                    prop.SetValue(p, null, null);
                                                else
                                                    prop.SetValue(p, 0, null);
                                            }
                                            else {
                                                Double val = 0;
                                                if (Double.TryParse(dictionary[key].ToString(), out val))
                                                    prop.SetValue(p, val, null);
                                                else
                                                    prop.SetValue(p, null, null);
                                            }
                                        }
                                        else {
                                            object v = dictionary[key];
                                            if (prop.PropertyType == typeof(string) && !(v is string)) {
                                                v = v?.ToString();
                                            }
                                            prop.SetValue(p, v, null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return p;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var p = (T)obj;
            IDictionary<string, object> serialized = new Dictionary<string, object>();

            foreach (var pi in typeof(T).GetProperties())
            {
                if (pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof(DateTime?))
                {
                    serialized[pi.Name] = ((DateTime)pi.GetValue(p, null)).ToString(_dateFormat);
                }
                else
                {

                    serialized[pi.Name] = pi.GetValue(p, null);
                }

            }

            return serialized;
        }

        public static JavaScriptSerializer GetSerializer()
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new AlbJsExtendConverter<T>() });

            return serializer;
        }

    }
}
