using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ALBINGIA.Framework.Common.Extensions {
    public static class Types {
        internal static ConcurrentDictionary<Enum, object> EnumsDataCache = new ConcurrentDictionary<Enum, object>();
        internal static ConcurrentDictionary<Type, Dictionary<object, Enum>> EnumsCache = new ConcurrentDictionary<Type, Dictionary<object, Enum>>();
        internal static ConcurrentDictionary<Enum, string> EnumNamesCache = new ConcurrentDictionary<Enum, string>();

        public static string DisplayName(this Enum @enum, string defaultName = "") {
            return EnumNamesCache.GetOrAdd(@enum, (value) => {
                var attribute = value.GetType().GetField(@enum.ToString())?.GetCustomAttribute(typeof(DisplayNameAttribute));
                if (attribute == null) {
                    attribute = value.GetType().GetField(@enum.ToString())?.GetCustomAttribute(typeof(DisplayAttribute));
                    return attribute == null ? defaultName : (((DisplayAttribute)attribute).Name ?? ((DisplayAttribute)attribute).Description ?? defaultName);
                }
                else {
                    return ((DisplayNameAttribute)attribute).DisplayName;
                }
            });
        }

        public static TCode AsCode<TAtt, TCode>(this Enum @enum, TCode defaultCode = default(TCode)) where TAtt : Attribute, ICodeAttribute<TCode> {
            return (TCode)EnumsDataCache.GetOrAdd(@enum, (value) => {
                var attribute = value.GetType().GetField(@enum.ToString())?.GetCustomAttribute(typeof(TAtt));
                return attribute == null ? defaultCode : ((TAtt)attribute).Code;
            });
        }

        /// <summary>
        /// Gets the Business Code of an enum value using the <see cref="BusinessCodeAttribute"/> assuming it is defined for the member
        /// </summary>
        /// <param name="enum">Enum value</param>
        /// <param name="defaultCode">Default code returned if no <see cref="BusinessCodeAttribute"/> is found and if setValueAsDefault is False</param>
        /// <param name="setValueAsDefault">Defines whether the string reprensentation of enum value is used as default code if supplied defaultCode is NULL</param>
        /// <returns></returns>
        public static string AsCode(this Enum @enum, string defaultCode = null, bool setValueAsDefault = true) {
            string code = @enum.AsCode<BusinessCodeAttribute, string>();
            if (code == default)
            {
                code = @enum.AsCode<AlbEnumInfoValue, string>(defaultCode);
            }
            if (code == defaultCode && setValueAsDefault) {
                    return @enum.ToString();
            }
            return code;
        }

        public static TEnum AsEnum<TEnum, TAtt, TCode>(this TCode code) where TAtt : Attribute, ICodeAttribute<TCode> where TEnum : struct {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum) {
                throw new ArgumentException("Supplied enum type is not Enum", nameof(TEnum));
            }
            else if (code == null) {
                return default(TEnum);
            }
            EnumsCache.GetOrAdd(enumType, (type) =>
                Enum.GetValues(enumType).Cast<Enum>().ToDictionary(
                    v => {
                        var attribute = enumType.GetField(v.ToString()).GetCustomAttribute(typeof(TAtt));
                        return attribute == null ?
                            (typeof(TCode) == typeof(string) ? v.ToString() : (object)default(TCode))
                            : ((TAtt)attribute).Code;
                    })
            ).TryGetValue(code, out Enum @enum);
            if (@enum == null) {
                return default(TEnum);
            }
            return (TEnum)(@enum as object);
        }

        public static TEnum ParseCode<TEnum>(this string code) where TEnum : struct
            => code.AsEnum<TEnum, BusinessCodeAttribute, string>();



        public static string GetDisplayString<T>(this T obj, Expression<Func<T, object>> propertyExpression) {
            var memberInfo = GetPropertyInformation(propertyExpression.Body);
            if (memberInfo == null) {
                throw new ArgumentException(
                    "No property reference expression was found.",
                    "propertyExpression");
            }

            var displayAttribute = memberInfo.GetAttribute<DisplayAttribute>(false);

            if (displayAttribute != null) {
                return displayAttribute.Name;
            }
            else {
                var displayNameAttribute = memberInfo.GetAttribute<DisplayNameAttribute>(false);
                if (displayNameAttribute != null) {
                    return displayNameAttribute.DisplayName;
                }
                else {
                    return memberInfo.Name;
                }
            }
        }

        public static IDictionary<string, object> GetDisplayStringsAndValues<T>(this T obj) {
            var properties = typeof(T).GetProperties().ToDictionary(p => p.Name);
            var displayAttributes = properties.Values.ToDictionary(p => p.Name, p => p.GetCustomAttributes<DisplayAttribute>());
            var displayNameAttributes = properties.Values.ToDictionary(p => p.Name, p => p.GetCustomAttributes<DisplayNameAttribute>());
            var noAttributeNames = properties.Values.Where(p => !displayAttributes.ContainsKey(p.Name) && !displayNameAttributes.ContainsKey(p.Name)).ToDictionary(p => p.Name);

            var namedValues = new Dictionary<string, object>();
            foreach (var key in displayAttributes.Keys) {
                namedValues.Add(displayAttributes[key].First().Name, properties[key].GetValue(obj));
            }
            foreach (var key in displayNameAttributes.Keys) {
                if (!displayAttributes.ContainsKey(key)) {
                    namedValues.Add(displayNameAttributes[key].First().DisplayName, displayNameAttributes[key]);
                }
            }
            foreach (var key in noAttributeNames.Keys) {
                namedValues.Add(key, noAttributeNames[key].GetValue(obj));
            }

            return namedValues;
        }

        public static T GetAttribute<T>(this MemberInfo member, bool isRequired) where T : Attribute {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired) {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static MemberInfo GetPropertyInformation(Expression propertyExpression) {
            //Debug.Assert(propertyExpression != null, "propertyExpression != null");
            MemberExpression memberExpr = propertyExpression as MemberExpression;
            if (memberExpr == null) {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert) {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property) {
                return memberExpr.Member;
            }

            return null;
        }

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static bool IsAnonymous(this Type type) => type != null
            && Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
            && type.IsGenericType && type.Name.Contains("AnonymousType")
            && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
            && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;

        public static bool IsValueTuple(this Type type) => type != null
            && type.IsValueType
            && Regex.IsMatch(type.Name, "^ValueTuple`");

        public static dynamic ToAnonymous(this IDictionary<string, object> properties) {
            var expando = new ExpandoObject();
            var collection = (ICollection<KeyValuePair<string, object>>)expando;
            foreach (var kvp in properties) {
                collection.Add(kvp);
            }
            return expando;
        }

        public static bool TryChangeType<T>(this object obj, out T result) {
            result = default;
            if (obj != null) {
                try {
                    result = (T)Convert.ChangeType(obj, typeof(T));
                    return true;
                }
                catch {
                    result = default;
                }
            }
            return false;
        }
    }
}
