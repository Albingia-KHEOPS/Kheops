using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using AjaxControlToolkit;

namespace Albingia.Hexavia.Web.Helper
{
    public static class OutilsHelper
    {
        /// <summary>
        /// Force le Type de Language utilisé (par Défaut: Français)
        /// </summary>
        /// <param name="date">date à convertir</param>
        /// <param name="format">format à utilisé</param>
        /// <returns>date formatée</returns>
        public static string FormatDateFrancais(DateTime? date, string format)
        {
            if (date == null)
            {
                return "";
            }
            else
            {
                DateTimeFormatInfo dtf = new CultureInfo("fr-FR", false).DateTimeFormat;
                return date.Value.ToString(format, dtf);
            }
        }

        /// <summary>
        /// Format avec la méthode FormatDateFrancais
        /// selon un affichage: MMM yyyy (Mai 2010)
        /// </summary>
        /// <param name="date">date à convertir</param>
        /// <returns>date formatée</returns>
        public static string FormatDate(DateTime? date)
        {
            return FormatDateFrancais(date, "MMM yyyy");
        }

        /// <summary>
        /// Format avec la méthode FormatDateFrancais
        /// selon un affichage: dd/MM/yyyy (01/05/2010)
        /// </summary>
        /// <param name="date">date à convertir</param>
        /// <returns>date formatée</returns>
        public static string FormatDateCalendar(DateTime? date)
        {
            return FormatDateFrancais(date, "dd/MM/yyyy");
        }

        public static DateTime? ToNullableDate(object date)
        {
            DateTime result;
            if (DateTime.TryParse(date.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static DateTime ToDate(object date)
        {
            DateTime result;
            if (DateTime.TryParse(date.ToString(), out result))
            {
                return result;
            }
            else
            {
                return new DateTime();
            }
        }

        public static DateTime? SetDateTime(DateTime? date, DateTime? heure)
        {
            DateTime? dateHeure = null;

            if (date.HasValue && heure.HasValue)
            {
                DateTime ldate = ToDate(date);
                DateTime lheure = ToDate(heure);

                return dateHeure = new DateTime(ldate.Year,
                                                    ldate.Month,
                                                   ldate.Day,
                                                    lheure.Hour,
                                                    lheure.Minute,
                                                    0);
            }
            return null;
        }

        public static bool ToBool(object obj)
        {
            bool result;
            if (Boolean.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return false;
            }
        }

        public static bool? ToNullableBool(object obj)
        {
            bool result;
            if (Boolean.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static int ToInt(object entier)
        {
            int result;
            if (Int32.TryParse(entier.ToString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Conversion d'une chaîne de caractère en décimal nullable
        /// </summary>
        /// <param name="dec">Chaîne de caractère à convertir</param>
        /// <returns>Decimal nullable</returns>
        public static Decimal? ToNullableDecimal(string dec)
        {
            Decimal result;
            if (Decimal.TryParse(dec, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static Decimal ToDecimal(string dec)
        {
            Decimal result;
            if (Decimal.TryParse(dec, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        public static int? ConvertToIntAS400(string nb, int nbCaractere)
        {
            if (nb.Length < Math.Pow(10, nbCaractere))
            {
                return Convert.ToInt32(nb);
            }
            return null;
        }

        public static void ChargerListeDeroulanteBlanc(DropDownList ddl, object dataSource, string valueField, string textField, string selText = "")
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = valueField;
            ddl.DataTextField = textField;
            ddl.DataBind();

            if (selText != "")
            {
                 ddl.Items.FindByValue("").Text = selText;
            }
        }

        public static void ChargerListeDeroulante(DropDownList ddl, object dataSource, string valueField, string textField, string selText = "")
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = valueField;
            ddl.DataTextField = textField;
            ddl.DataBind();

            if (selText != "")
            {
                ddl.Items.Insert(0, new ListItem(selText, "-1"));
            }
        }

        public static void ChargerListeDeroulante(ComboBox ddl, object dataSource, string valueField, string textField, string selText = "")
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = valueField;
            ddl.DataTextField = textField;
            ddl.DataBind();

            if (selText != "")
            {
                ddl.Items.Insert(0, new ListItem(selText, "-1"));
            }
        }
        public static void ChargerListeDeroulante(RadComboBox ddl, object dataSource, string valueField, string textField, string selText = "")
        {
            ddl.DataSource = dataSource;
            ddl.DataValueField = valueField;
            ddl.DataTextField = textField;
            ddl.DataBind();

            if (selText != "")
            {
                ddl.Items.Insert(0, new RadComboBoxItem(selText, "-1"));
            }
        }
    }
}