using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain.Helper
{
    public class DateHelper
    {
        public static DateTime? AS400GetDate(int annee, int mois, int jour, int horaire)
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

        //Conversion for Database format
        public static int AS400GetDate(DateTime date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }

        //public static int ConvertHourAndMinute(DateTime? date)
        //{
        //    int result = 0;
        //    if (date.HasValue)
        //    {
        //        result = date.Value.Hour * 100 + date.Value.Minute;
        //    }
        //    return result;
        //}
    }
}
