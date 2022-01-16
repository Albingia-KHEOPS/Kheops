using ALBINGIA.Framework.Common;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
//using System.Web.ModelBinding;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.CustomModelBinders
{
    /// <summary>
    /// Override for DefaultModelBinder in order to implement fixes to its behavior.
    /// This model binder inherits from the default model binder. All this does is override the default one,
    /// check if the property is an enum, if so then use custom binding logic to correctly map the enum. If not,
    /// we simply invoke the base model binder (DefaultModelBinder) and let it continue binding as normal.
    /// </summary>
    public class AlbModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Fix for the default model binder's failure to decode enum types when binding to JSON.
        /// </summary>
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            Type propertyType = propertyDescriptor.PropertyType;
            var providerValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (propertyType.IsEnum)
            {
                var result = ProcessEnumValue(providerValue, propertyType);
                if (result.ok)
                {
                    return result.value;
                }
            }
            else if (Nullable.GetUnderlyingType(propertyType) != null)
            {
                var result = ProcessNullableValue(providerValue, propertyType);
                if (result.ok)
                {
                    return result.value;
                }
            }
            else if (propertyType == typeof(string))
            {
                bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
            }

            var returnValue = base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
            return returnValue;
        }

        internal static (bool ok, object value) ProcessEnumValue(ValueProviderResult providerValue, Type propertyType)
        {
            if (null != providerValue)
            {
                var value = providerValue.RawValue;
                if (null != value)
                {
                    var valueType = value.GetType();
                    try
                    {
                        string stringValue = value as string;
                        if (stringValue == null && valueType == typeof(string[]) && ((string[])value).Length == 1)
                        {
                            stringValue = ((string[])value)[0];
                        }

                        if (stringValue != null)
                        {
                            if (int.TryParse(stringValue, out int i))
                            {
                                return (true, Enum.ToObject(propertyType, i));
                            }
                            else
                            {
                                var ev = Enum.GetValues(propertyType)
                                  .Cast<Enum>()
                                  .FirstOrDefault(en => en.GetAttributeOfType<DisplayAttribute>()?.Name == stringValue);

                                if (ev == null)
                                {
                                    ev = Enum.GetValues(propertyType)
                                        .Cast<Enum>()
                                        .FirstOrDefault(en => en.GetAttributeOfType<BusinessCodeAttribute>()?.Code == stringValue);
                                }
                                return (true, ev != null ? ev : Enum.Parse(propertyType, stringValue, true));
                            }
                        }
                        else if (Nullable.GetUnderlyingType(propertyType) != null)
                        {
                            return (true, null);
                        }
                        else if (!valueType.IsEnum)
                        {
                            return (true, Enum.ToObject(propertyType, value));
                        }
                    }
                    catch (Exception) { }
                }
            }

            return (false, null);
        }

        internal static (bool ok, object value) ProcessNullableValue(ValueProviderResult providerValue, Type propertyType)
        {
            if (null != providerValue)
            {
                var value = providerValue.RawValue;
                if (value is string[] array)
                {
                    if (array.Length == 1 || array.All(x => bool.TryParse(x, out var b)))
                    {
                        value = array.First();
                    }
                }

                Type valueType = Nullable.GetUnderlyingType(propertyType);
                if (null == value)
                {
                    return (true, null);
                }
                else if (value.GetType() == valueType)
                {
                    return (true, value);
                }
                else if (valueType?.IsEnum ?? propertyType.IsEnum)
                {
                    return ProcessEnumValue(providerValue, valueType ?? propertyType);
                }

                try
                {
                    if (value is string stringValue)
                    {
                        if (string.IsNullOrWhiteSpace(stringValue))
                        {
                            return (false, null);
                        }
                        MethodInfo m = valueType.GetMethod("TryParse", new[] { typeof(string), valueType });
                        if (m != null)
                        {
                            var param = new[] { value, null };
                            if ((bool)m.Invoke(null, param))
                            {
                                return (true, param[1]);
                            }
                            else
                            {
                                return (false, null);
                            }
                        }
                        m = valueType.GetMethod("Parse", new[] { typeof(string) });
                        if (m != null)
                        {
                            var res = m.Invoke(null, new[] { value });
                            return (true, res);
                        }
                        return (true, Convert.ChangeType(value, valueType));
                    }
                    return (true, propertyType.GetProperty("Value").GetValue(Convert.ChangeType(value, valueType)));
                }
                catch { }
            }

            return (false, null);
        }
    }
}