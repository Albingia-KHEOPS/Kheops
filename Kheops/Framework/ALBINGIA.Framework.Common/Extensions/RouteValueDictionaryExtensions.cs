using System;
using System.Collections;
using System.Web.Routing;

namespace ALBINGIA.Framework.Common.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary ToRouteValueDictionaryWithCollection(this RouteValueDictionary routeValues)
        {
            RouteValueDictionary newRouteValues = new RouteValueDictionary();

            foreach (var key in routeValues.Keys)
            {
                object value = routeValues[key];

                if (value is IEnumerable && !(value is string))
                {
                    int index = 0;
                    foreach (object val in (IEnumerable)value)
                    {
                        if (val is string || val.GetType().IsPrimitive)
                        {
                            newRouteValues.Add(String.Format("{0}[{1}]", key, index), val);
                        }
                        else
                        {
                            var properties = val.GetType().GetProperties();
                            foreach (var propInfo in properties)
                            {
                                newRouteValues.Add(
                                    String.Format("{0}[{1}].{2}", key, index, propInfo.Name),
                                    propInfo.GetValue(val));
                            }
                        }
                        index++;
                    }
                }
                else if (value != null && !(value is string) && !value.GetType().IsPrimitive && !(value is Enum))
                {
                    var properties = value.GetType().GetProperties();
                    foreach (var propInfo in properties)
                    {
                        newRouteValues.Add(
                            String.Format("{0}.{1}", key, propInfo.Name),
                            propInfo.GetValue(value));
                    }
                }
                else
                {
                    newRouteValues.Add(key, value);
                }
            }

            return newRouteValues;
        }
    }
}
