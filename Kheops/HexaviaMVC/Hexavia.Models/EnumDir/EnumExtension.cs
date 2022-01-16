using System;
using System.ComponentModel;

namespace Hexavia.Models.EnumDir
{
    public static class EnumExtension
    {
        /// <summary>
        /// Retrieve the description on the enum
        /// </summary>
        /// <param name="enumeration">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string ToDescription(this System.Enum enumeration)
        {
            if (enumeration == null)
            {
                throw new ArgumentNullException(nameof(enumeration));
            }

            var type = enumeration.GetType();

            var memInfo = type.GetMember(enumeration.ToString());

            if (memInfo.Length <= 0)
            {
                return enumeration.ToString();
            }

            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0
                ? ((DescriptionAttribute)attrs[0]).Description
                : enumeration.ToString();
        }

        /// <summary>
        /// Retrieve enum value from description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">description</exception>
        /// <exception cref="System.InvalidOperationException">T is not Enum type</exception>
        public static T FromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("T is not Enum type");
            }

            if (description == null)
            {
                return default(T);
            }

            foreach (var field in type.GetFields())
            {
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }

                    continue;
                }

                if (field.Name == description)
                {
                    return (T)field.GetValue(null);
                }
            }

            return default(T);
        }
    }
}
