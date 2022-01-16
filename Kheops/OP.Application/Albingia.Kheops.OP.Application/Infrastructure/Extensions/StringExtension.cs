using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Infrastructure.Extension
{
    public static class Tools
    {
        public static bool IsSameAs(this string a, string b)
        {
            a = a?.Trim();
            b = b?.Trim();
            return String.Compare(a, b, true) == 0;
        }

        public static string AsIPB(this string a) => a.PadLeft(9, ' ');
        public static string ToYesNo(this bool value, string yes, string no) => value ? yes : no;
        public static string ToYesNo(this bool value) => value ? "O" : "N";
        public static string ToYesNo(this bool? value) => value.HasValue ? (value.Value ? "O" : "N") : "";

        public static string ToFrenchPostalCode(this int cp) => cp.ToString("00000");

        public static string MakeCodePostal(int dep, int ext) => dep.ToString("00") + ext.ToString("000");

        public static string JoinString<T>(this IEnumerable<T> source, Func<T, string> projector, string separator) => String.Join(separator, source.Select(projector));
        public static string JoinString(this IEnumerable<string> source, string separator) => String.Join(separator, source);

        public static bool? AsNullableBool(this string value) => String.IsNullOrWhiteSpace(value) ? null : (bool?)value.AsBool();
        public static Int64 AsLong(this string value) => long.Parse(value);
        public static Int64 AsLong(this int value) => (long)value;
        //public static decimal AsDecimal(this double value) => (decimal) value;
        //public static double AsDouble(this decimal value) => (double) value;
        public static decimal AsDecimal(this decimal value) => (decimal)value;
        public static decimal AsDouble(this decimal value) => (decimal)value;
        public static (int date, int hour) AsDateHour(this DateTime value)
        {
            return (value.AsDateNumber(), value.AsTimeNumber());
        }

        public static int AsTime4(this DateTime? value) => value.HasValue ? value.Value.AsTimeNumber() : 0;
        public static int AsTime6(this DateTime? value) => value.HasValue ? value.Value.AsTimeNumber6() : 0;
        public static int AsDate(this DateTime? value) => value.HasValue ? value.Value.AsDateNumber() : 0;
        public static (int date, int hour) AsDateHour(this DateTime? value)
        {
            return (value.AsDate(), value.AsTime4());
        }


        public static bool AsBool(this string value, string trueValue) => value.IsSameAs(trueValue);
        public static bool AsBool(this string value) => value.AsBool("O");

        public static int NYear(this DateTime? value) => value?.Year ?? 0;
        public static int NMonth(this DateTime? value) => value?.Month ?? 0;
        public static int NDay(this DateTime? value) => value?.Day ?? 0;
        public static int NTime(this DateTime? value) => (value?.Hour * 100 ?? 0) + (value?.Minute ?? 0);

        public static DateTime? MakeNullableDateTime(int a, int m, int d, int h = 0, int min = 0, int s = 0)
        {
            return a == 0 ? null : (DateTime?)MakeDateTime(a, m, d, h, min, s);
        }

        public static DateTime? MakeNullableDateTime(int date, int time = 0, bool isTime6 = false)
        {
            if (isTime6) {
                string stime = time.ToString().PadLeft(6, '0');
                return MakeNullableDateTime(
                    date / 10000,
                    (date / 100) % 100,
                    date % 100,
                    int.Parse(stime.Substring(0, 2)),
                    int.Parse(stime.Substring(2, 2)),
                    int.Parse(stime.Substring(4, 2)));
            }
            return MakeNullableDateTime(date / 10000, (date / 100) % 100, date % 100, time);
        }
        public static DateTime MakeDateTime(int a, int m, int d, int h = 0, int min = 0, int s = 0)
        {
            var hour = h;
            var minute = min;
            var second = s;
            if (h >= 2400) {
                hour = h / 10000;
                minute = (h / 100) % 100;
                second = h % 100;
            } else if (h > 24) {
                minute = h % 100;
                hour = h / 100;
            }
            if (a > 0 && a<=9999 && m >= 1 && m <= 12 && d >= 1 && d <= 31 && hour >= 0 && hour <= 24 && minute >= 0 && minute <= 60 && second >= 0 && second <= 60)
            {
                try
                {
                    return new DateTime(a, m, d, hour, minute, second);
                }
                catch(ArgumentOutOfRangeException)  {}
            }
            return DateTime.MinValue;
        }
        public static DateTime MakeDateTime(int date, int time = 0)
        {
            return MakeDateTime(date / 10000, (date / 100) % 100, date % 100, time);
        }
        public static int AsDateNumber(this DateTime? date) {
            return date.HasValue ? AsDateNumber(date.Value) : 0;
        }
        public static int AsDateNumber(this DateTime date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }
        public static int AsTimeNumber(this DateTime? date) {
            return date.HasValue ? AsTimeNumber(date.Value) : 0;
        }
        public static int AsTimeNumber(this DateTime date)
        {
            return date.Hour * 100 + date.Minute;
        }
        public static int AsTimeNumber6(this DateTime? date)
        {
            return date.HasValue ? AsTimeNumber(date.Value) : 0;
        }
        public static int AsTimeNumber6(this DateTime date)
        {
            return date.Hour * 10000 + date.Minute * 100 + date.Second;
        }
    }
}
