using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EasycomClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataAccess.Helpers
{
    public static class OutilsHelper
    {
        public static readonly Regex ColunmNameRegex = new Regex(@"^[A-Z][A-Z0-9_]*$", RegexOptions.Compiled | RegexOptions.Singleline);
        public static readonly Regex DtoMemberNameRegex = new Regex(@"^[A-Z]\w*$", RegexOptions.Compiled | RegexOptions.Singleline);

        public static string If(bool condition, string value) => condition ? value : String.Empty;

        public static int? ToNullableInt(string entier)
        {
            if (int.TryParse(entier, out int result)) {
                return result;
            }
            else {
                return null;
            }
        }

        public static bool ContientLeChampEtEstNonNull(DataRow ligne, string NomChamp)
        {
            bool result = false;
            if (NomChamp == "TCOD") {
                result = ligne.Table.Columns.Contains(NomChamp);
            }
            else {
                result = ligne.Table.Columns.Contains(NomChamp) && !string.IsNullOrEmpty(ligne[NomChamp].ToString());
            }
            return result;
        }

        public static bool ContientUnDesChampsEtEstNonNull(DataRow ligne, params string[] champs)
        {
            bool result = false;
            for (int i = 0; i < champs.Length; i++) {
                result = result || ContientLeChampEtEstNonNull(ligne, champs[i]);
            }
            return result;
        }

        public static string ParseCorrectlyAddressString(string addressElement)
        {
            if (!string.IsNullOrEmpty(addressElement)) {
                if (addressElement.Trim().Replace("0", "") != string.Empty) {
                    return addressElement.Trim();
                }
            }

            return string.Empty;
        }


        private static readonly Regex paramsExpression = new Regex(@"\:(\w+)");
        private static IEnumerable<string> GetParamNames(string sql)
        {
            HashSet<string> seen = new HashSet<string>();
            var match = paramsExpression.Match(sql);
            while (match.Success) {
                var name = match.Groups[1].Value;
                if (!seen.Contains(name)) {
                    seen.Add(name);
                    yield return name;
                }
                match = match.NextMatch();
            }
        }

        public static IEnumerable<EacParameter> MakeParams(string sql, IEnumerable<object> paramValues)
        {
            return MakeParams(sql, paramValues.ToArray());
        }
        public static IEnumerable<EacParameter> MakeParams(string sql, params object[] paramValues)
        {
            List<EacParameter> parameters = new List<EacParameter>();
            var names = GetParamNames(sql).ToList();
            if (paramValues.Length != names.Count()) {
                throw new ArgumentException("Nombre de paramètres incorrect", nameof(paramValues));
            }
            for (var i = 0; i < names.Count; i++) {
                var name = names[i];
                yield return new EacParameter(name, paramValues[i]);
            }
        }
        public static IEnumerable<EacParameter> MakeParamsDynamic(string sql, object paramValues, bool loose = false)
        {
            List<EacParameter> parameters = new List<EacParameter>();
            var names = GetParamNames(sql).ToList();
            var properties = paramValues.GetType().GetProperties().ToDictionary(x => x.Name);
            if (!loose && properties.Count != names.Count()) {
                throw new ArgumentException("Nombre de paramètres incorrect", nameof(paramValues));
            }
            for (var i = 0; i < names.Count; i++) {
                var name = names[i];
                var value = properties[name].GetGetMethod().Invoke(paramValues, new object[] { });
                yield return new EacParameter(name, value);
            }
        }

        public static (string sql, IEnumerable<EacParameter> parameters) MakeParamsSql(string sql, IEnumerable<object> paramValues)
        {
            return (sql, MakeParams(sql, paramValues));
        }

        public static (string sql, IEnumerable<EacParameter> parameters) MakeParamsSql(string sql, params object[] paramValues)
        {
            return (sql, MakeParams(sql, paramValues));
        }

        public static IEnumerable<EacParameter> MakeParams(string sql, Dictionary<string, object> paramValues, bool loose = false)
        {
            List<EacParameter> parameters = new List<EacParameter>();
            var names = GetParamNames(sql).ToList();
            if (!loose && paramValues.Count() != names.Count()) {
                throw new ArgumentException("Nombre de paramètres incorrect", nameof(paramValues));
            }
            foreach (var name in names) {
                if (!paramValues.ContainsKey(name)) {
                    throw new KeyNotFoundException($"Paramètre '{name}' non trouvé");
                }
                yield return new EacParameter(name, paramValues[name]);
            }
        }
        public static (string sql, IEnumerable<EacParameter> parameters) MakeParamsSql(string sql, Dictionary<string, object> paramValues)
        {
            return (sql, MakeParams(sql, paramValues));
        }

        public static string MakeCastTimestamp(string yearname, string monthname, string dayname, string hourname = "0000") {
            var numbers = new Dictionary<string, bool> {
                { nameof(yearname), Regex.IsMatch(yearname, "^(\\w+\\.)?\\w+$") },
                { nameof(monthname), Regex.IsMatch(monthname, "^(\\w+\\.)?\\w+$") },
                { nameof(dayname), Regex.IsMatch(dayname, "^(\\w+\\.)?\\w+$") },
                { nameof(hourname), Regex.IsMatch(hourname, "^(\\w+\\.)?\\w+$") }
            };
            if (numbers.Any(x => !x.Value)) {
                throw new ArgumentException(string.Join(",", numbers.Where(x => !x.Value).Select(x => x.Key)));
            }
            return $"CAST ( LPAD ( {yearname} , 4 , '0' ) || LPAD ( {monthname} , 2 , '0' ) || LPAD ( {dayname} , 2 , '0' ) || RPAD ( LPAD ( {hourname} , 4 , '0' ) , 6 , '0' ) AS TIMESTAMP )";
        }

        public static string MakeCastTimestamp(string prefix, bool includeHour = false) {
            if (prefix is null || !Regex.IsMatch(prefix, "^[\\w\\.]+$")) {
                throw new ArgumentException(nameof(prefix));
            }
            return MakeCastTimestamp(prefix + "A", prefix + "M", prefix + "J", includeHour ? (prefix + "H") : "0");
        }

        public static DateTime? MakeDateTimeFromAMJH<T>(this T as400Dto, string propYearName, bool allowDayZero = true) where T : class {
            if (as400Dto == default(T)) {
                return null;
            }
            if (propYearName is null) {
                throw new ArgumentNullException(nameof(propYearName));
            }
            if (!DtoMemberNameRegex.IsMatch(propYearName) || propYearName.Last() != 'a') {
                throw new ArgumentException(nameof(propYearName));
            }
            string prefix = propYearName.Substring(0, propYearName.Length - 1);
            var objMembers = TypeAccessor.Create(typeof(T)).GetMembers().Where(m => m.Name.StartsWith(prefix));
            if (!objMembers.Any()) {
                throw new ArgumentException($"The prefix does not match with any of the {typeof(T).Name} members", nameof(prefix));
            }
            var obj = ObjectAccessor.Create(as400Dto);
            int year = (int)obj[propYearName];
            if (year == 0) {
                return null;
            }
            int month = 0, day = 0, hour = 0, minute = 0, second = 0;
            if (objMembers.Any(m => m.Name == $"{prefix}m")) {
                month = (int)obj[$"{prefix}m"];
            }
            if (objMembers.Any(m => m.Name == $"{prefix}j")) {
                day = (int)obj[$"{prefix}j"];
                if (day < 1) {
                    if (allowDayZero) {
                        day = 1;
                    }
                    else {
                        throw new FormatException($"{prefix}j was expected being greater than 0");
                    }
                }
            }
            if (objMembers.Any(m => m.Name == $"{prefix}h")) {
                hour = (int)obj[$"{prefix}h"];
                string hourStr = hour.ToString();
                if (hourStr.Length < 5) {
                    hourStr = hourStr.PadLeft(4, '0');
                }
                else {
                    hourStr = hourStr.PadLeft(6, '0');
                    second = int.Parse(hourStr.Substring(4, 2));
                }
                hour = int.Parse(hourStr.Substring(0, 2));
                minute = int.Parse(hourStr.Substring(2, 2));
            }
            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
