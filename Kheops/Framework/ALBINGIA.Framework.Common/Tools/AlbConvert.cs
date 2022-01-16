using ALBINGIA.Framework.Common.Constants;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ALBINGIA.Framework.Common.Tools
{
    public static class AlbConvert
    {
        /// <summary>
        /// Defines global application culture info (French)
        /// </summary>
        public static readonly CultureInfo AppCulture = new CultureInfo("fr-FR");

        /// <summary>
        /// Defines a global short date pattern used as ToString parameter (dd/MM/yyyy)
        /// </summary>
        public static readonly string DateFormat = AppCulture.DateTimeFormat.ShortDatePattern;

        public static int? ConvertDateToInt(DateTime? date)
        {
            if (date == null) {
                return null;
            }

            return date.Value.Year * 10000 + date.Value.Month * 100 + date.Value.Day;
        }

        public static string ToString<T>(this Nullable<T> nullable, string format, bool emptyIfNoValue = true) where T: struct {
            try {
                return nullable.HasValue ?
                    nullable.Value.GetType().InvokeMember("ToString", BindingFlags.InvokeMethod, null, nullable.Value, new object[] { format }) as string
                    : emptyIfNoValue ? string.Empty : null;
            }
            catch (Exception e) when (e is MissingMethodException || e is InvalidOperationException) {
                return null;
            }
        }

        public static string ConvertDateToStr(DateTime? date)
        {
            if (date == null)
                return string.Empty;

            return string.Format("{0}/{1}/{2}",
                    date.Value.Day.ToString().PadLeft(2, '0'),
                    date.Value.Month.ToString().PadLeft(2, '0'),
                    date.Value.Year.ToString());
        }

        public static string ToStringYMD(this DateTime date, char separator = default(char))
        {
            var validSeparatorChars = new char[] { '-', '/', '.' };
            if (validSeparatorChars.Contains(separator))
            {
                return date.ToString($"yyyy{separator}MM{separator}dd");
            }

            return date.ToString("yyyyMMdd");
        }

        public static int ToIntYMD(this DateTime date)
        {
            if (date == DateTime.MinValue) {
                return 0;
            }
            return int.Parse(date.ToStringYMD());
        }

        public static int ToIntYMD(this DateTime? date, int defaultInt = 0) {
            return date.GetValueOrDefault() == default(DateTime) ? defaultInt : date.Value.ToIntYMD();
        }

        public static int ToIntHMS(this DateTime date) {
            return date.Hour * 10000 + date.Minute * 100 + date.Second;
        }

        public static int ToIntHMS(this DateTime? date, int defaultInt = 0) {
            return date.GetValueOrDefault() == default(DateTime) ? defaultInt : int.Parse(date.Value.ToString("HHmmss"));
        }

        public static int ToIntHM(this DateTime? date, int defaultInt = 0) {
            return date.GetValueOrDefault() == default(DateTime) ? defaultInt : int.Parse(date.Value.ToString("HHmm"));
        }
        public static int ToIntHM(this DateTime date, int defaultInt = 0)
        {
            return int.Parse(date.ToString("HHmm"));
        }


        public static DateTime? YMDToDate(this int dateInt)
        {
            if (dateInt < 1) {
                return null;
            }
            try
            {
                int year = (int)Math.Truncate(dateInt / 10000M);
                int month = (int)Math.Truncate(((dateInt / 10000M) - year) * 100M);
                int day = (int)(((dateInt / 100M) - Math.Truncate(dateInt / 100M)) * 100M);
                return new DateTime(year, month, day);
            }
            catch
            {
                return null;
            }
        }

        public static TimeSpan? ConvertIntToTime(int? time)
        {
            if (time == null)
                return null;

            TimeSpan retTime = TimeSpan.ParseExact(time.HasValue ? time.Value.ToString().PadLeft(6, '0') : string.Empty, "hhmmss", CultureInfo.CurrentCulture, TimeSpanStyles.None);

            return retTime;
        }

        public static int? ConvertTimeToInt(TimeSpan? time)
        {
            if (time == null)
                return null;

            return time.Value.Hours * 10000 + time.Value.Minutes * 100 + time.Value.Seconds;
        }
        public static int? ConvertTimeMinuteToInt(TimeSpan? time)
        {
            if (time == null)
                return null;

            return time.Value.Hours * 100 + time.Value.Minutes;
        }
        public static TimeSpan? ConvertIntToTimeMinute(int? time)
        {
            if (time == null)
                return null;

            TimeSpan retTime = TimeSpan.ParseExact(time.HasValue ? time.Value.ToString().Length > 4 ? time.Value.ToString().Substring(0, 4) : time.Value.ToString().PadLeft(4, '0') : string.Empty, "hhmm", CultureInfo.CurrentCulture, TimeSpanStyles.None);

            return retTime;
        }

        public static int? ConvertTimeToIntMinute(TimeSpan? time)
        {
            if (time == null)
                return null;

            return time.Value.Hours * 100 + time.Value.Minutes;
        }

        public static TimeSpan? GetTimeFromDate(DateTime? date)
        {
            if (date == null)
                return null;

            return new TimeSpan(date.Value.Hour, date.Value.Minute, date.Value.Second);
        }

        public static DateTime? ConvertIntToDate(int? date)
        {
            if (date == null)
                return null;

            DateTime castDate;
            DateTime.TryParseExact(date.HasValue ? date.Value.ToString() : string.Empty, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out castDate);
            DateTime? retDate = castDate;

            return retDate == DateTime.MinValue ? null : retDate;
        }

        public static DateTime? ConvertIntToDateHour(Int64? date)
        {
            if (date == null || !date.HasValue || date == 0)
                return null;

            double year = Math.Truncate(Convert.ToDouble(date.Value / 100000000));
            double month = Math.Truncate(Convert.ToDouble((date.Value - (year * 100000000)) / 1000000));
            double day = Math.Truncate(Convert.ToDouble((date.Value - (year * 100000000) - (month * 1000000)) / 10000));
            double hour = Math.Truncate(Convert.ToDouble((date.Value - (year * 100000000) - (month * 1000000) - (day * 10000)) / 100));
            double minute = (date.Value - (year * 100000000) - (month * 1000000) - (day * 10000) - (hour * 100));

            DateTime castDate = new DateTime(Convert.ToInt32(year), month == 0 ? 1 : Convert.ToInt32(month), day == 0 ? 1 :Convert.ToInt32(day), Convert.ToInt32(hour), Convert.ToInt32(minute), 0);

            return castDate;
        }

        public static DateTime? ConvertStrToDate(string date)
        {
            if (string.IsNullOrEmpty(date))
                return null;

            DateTime castDate;
            DateTime.TryParse(date, out castDate);

            DateTime? retDate = castDate;

            if (retDate == DateTime.MinValue)
                return null;
            return retDate;
        }

        public static int ConvertStrToIntHour(string hour)
        {
            if (string.IsNullOrEmpty(hour))
                return 0;

            string[] tHour = hour.Split(':');
            string strHour = string.Format("{0}{1}", tHour[0], tHour[1]);

            int retHour;
            int.TryParse(strHour, out retHour);

            return retHour;
        }

        public static bool ControlEqualDate(DateTime? date1, DateTime? date2)
        {
            bool toReturn = false;
            if (!date1.HasValue || !date2.HasValue) return false;

            if (AlbConvert.ConvertDateToInt(date1.Value) == AlbConvert.ConvertDateToInt(date2.Value))
            {
                toReturn = true;
            }
            return toReturn;
        }

        public static bool? ConvertStringToBool(string boolString)
        {
            if (boolString == null)
            {
                return null;
            }
            if (boolString.Equals("O"))
            {
                return true;
            }
            if (boolString.Equals("N"))
            {
                return false;
            }
            return null;
        }

        public static string ConvertBoolToString(bool? boolean)
        {
            if (boolean == null)
            {
                return null;
            }
            if (Convert.ToBoolean(boolean))
            {
                return "O";
            }
            return "N";
        }
        public static DateTime? GetDate(int annee, int mois, int jour, int horaire)
        {
            DateTime? result;

            if (annee == 0 || mois == 0 || jour == 0)
            {
                result = null;
            }
            else
            {
                int heure = horaire / 100;
                int minute = horaire - (100 * heure);
                result = new DateTime(annee, mois, jour, heure, minute, 0);
            }
            return result;
        }

        public static DateTime? GetFinPeriode(DateTime? debut, int duree, string unite)
        {
            if (debut.HasValue)
            {
                DateTime? toReturn = null;
                switch (unite)
                {
                    case AlbOpConstants.Jour:
                        toReturn = debut.Value.AddDays(duree).AddDays(-1); break;
                    case AlbOpConstants.Mois:
                        toReturn = debut.Value.AddMonths(duree).AddDays(-1); break;
                    case AlbOpConstants.Annee:
                        toReturn = debut.Value.AddYears(duree).AddDays(-1); break;
                }
                return toReturn.HasValue ? toReturn.Value.AddHours(23).AddMinutes(59) : toReturn;
            }
            return null;
        }

        /// <summary>
        /// Find the (int)val+1 
        /// </summary>
        /// <param name="val">the value to transform </param>
        /// <returns>(int)val+1</returns>
        public static int ConvertDoubleToFirstGreaterInt(double val)
        {
            int res = 0;

            res = (int)val;
            if (val != (double)res)
                res += 1;

            if (res == 0)
                res = 1;

            return res;
        }

    }
}
