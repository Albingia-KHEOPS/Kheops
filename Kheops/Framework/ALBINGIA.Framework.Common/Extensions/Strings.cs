using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ALBINGIA.Framework.Common.Extensions
{
    public static class StringExt
    {

        private const string FICHIER_NON_EXISTANT = "Le fichier n'existe pas";

        public static byte[] GetFileData(this string fileName, string filePath)
        {
            var fullFilePath = Path.Combine(filePath, fileName);
            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException(FICHIER_NON_EXISTANT, fullFilePath);
            return File.ReadAllBytes(fullFilePath);
        }

        public static string EmptyIfNull(this string @string)
        {
            return string.IsNullOrWhiteSpace(@string) ? string.Empty : @string;
        }

        /// <summary>
        /// Defines whether the string is NULL or does not contain any character
        /// </summary>
        /// <param name="string">The string</param>
        /// <param name="considerWhiteSpaces">Defnies whether the space characters are not ignored</param>
        /// <returns></returns>
        public static bool IsEmptyOrNull(this string @string, bool considerWhiteSpaces = false)
        {
            return considerWhiteSpaces && string.IsNullOrEmpty(@string) || string.IsNullOrWhiteSpace(@string);
        }

        public static string NullIfEmpty(this string s, bool considerWhiteSpaces = false) {
            return s is null || (!string.IsNullOrWhiteSpace(s) || considerWhiteSpaces && !string.IsNullOrEmpty(s)) ? s : null;
        }

        /// <summary>
        /// Defines whether the string is not NULL and contains at least one character
        /// </summary>
        /// <param name="string">The string</param>
        /// <param name="considerWhiteSpaces">Defnies whether the space characters are not ignored</param>
        /// <returns></returns>
        public static bool ContainsChars(this string @string, bool considerWhiteSpaces = false) {
            return !@string.IsEmptyOrNull(considerWhiteSpaces);
        }

        public static string OrDefault(this string @string, string @default)
        {
            return @string.IsEmptyOrNull() ? @default : @string;
        }

        public static string StringValue<T>(this T? nullable, string defaultString = "", bool defaultAsNull = false) where T: struct {
            if (defaultAsNull) {
                return Equals(nullable.GetValueOrDefault(), default(T)) ? defaultString : nullable.Value.ToString();
            }
            else {
                return nullable.HasValue ? nullable.Value.ToString() : defaultString;
            }
        }

        public static string ToIPB(this string code) => code.TrimEnd().PadLeft(9, ' ');

        /// <summary>
        /// Forces the string.Empty value if string.IsNullOrWhiteSpace() returns true
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns></returns>
        public static string EnsureEmpty(this string s) => string.IsNullOrWhiteSpace(s) ? string.Empty : s;

        public static bool? AsBoolean(this string sbool, bool emptyIsFalse = true) {
            if (sbool == null) {
                return null;
            }
            if (sbool.Trim().Length == 0) {
                return emptyIsFalse ? false : default(bool?);
            }
            if (sbool.IsIn("o", "y", true.ToString().ToLower(), "1")) {
                return true;
            }
            else if (sbool.IsIn("n", false.ToString().ToLower(), byte.MinValue.ToString())) {
                return false;
            }

            return null;
        }

        public static bool ContainsAny(this string @value, IEnumerable<string> testedAgainst) {
            return testedAgainst.Any(tested => @value.Contains(tested));
        }

        public static string[] CutKamelCase(this string s) {
            if ((s?.Trim().Length ?? 0) == 0) {
                return new string[0];
            }
            var list = new List<char>();
            for (int x = 0; x < s.Length; x++) {
                if (char.IsWhiteSpace(s[x])) continue;
                if (char.IsUpper(s[x])) {
                    list.Add('/');
                    list.Add(';');
                    list.Add('/');
                }
                list.Add(s[x]);
            }
            return (new string(list.ToArray())).Split(new[] { "/;/" }, StringSplitOptions.None );
        }

        public static string BoolToYesNo(this bool value) => value ? "O" : "N";
    }
}
