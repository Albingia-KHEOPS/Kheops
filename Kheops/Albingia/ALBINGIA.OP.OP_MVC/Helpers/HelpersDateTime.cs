using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ALBINGIA.Framework.Common.Tools;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Helpers
{
    public static class HelpersDateTime
    {
        /// <summary>
        /// définit le format de la date dd/MM/yyyy
        /// </summary>
        /// <typeparam name="TModel">Modele</typeparam>
        /// <typeparam name="TValue">Valeur</typeparam>
        /// <param name="helper">Html helper</param>
        /// <param name="expression">Lambda expression utilisée dans le Html Helper d'origine</param>
        /// <param name="htmlAttributes"> Les attributs Html</param>
        /// <returns>Date</returns>
        public static MvcHtmlString AlbDateFormat<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null, string dateFormat = "dd/MM/yyyy", string htmlName = "")
        {
           
            var function = expression.Compile();
            var result = function(helper.ViewData.Model);
            var fieldName = !string.IsNullOrEmpty(htmlName) ? htmlName : AlbTools.GetFieldName(helper, expression);
            if (result == null)
                return htmlAttributes != null ? helper.TextBox(fieldName, MvcHtmlString.Empty, htmlAttributes) : helper.TextBox(fieldName, MvcHtmlString.Empty);
            if (!IsNullable(result))
                return htmlAttributes != null ? helper.TextBox(fieldName, Convert.ToDateTime(result).ToString(dateFormat), htmlAttributes) : helper.TextBox(fieldName, Convert.ToDateTime(result).ToString(dateFormat));
            var resHtml= htmlAttributes != null ? helper.TextBox(fieldName, Convert.ToDateTime(result).ToString(dateFormat), htmlAttributes).ToString() : helper.TextBox(fieldName, Convert.ToDateTime(result).ToString(dateFormat)).ToString();
           
            return SetValueToInput<TValue>(result, resHtml,dateFormat);
        }

       

        /// <summary>
        /// Set the Time Format HH:mm
        /// </summary>
        /// <typeparam name="TModel">Modele</typeparam>
        /// <typeparam name="TValue">Valeur</typeparam>
        /// <param name="helper">Html helper</param>
        /// <param name="expression">Lambda expression utilisée dans le Html Helper d'origine</param>
        /// <param name="htmlAttributes"> Les attributs Html</param>
        /// <returns>heure formattée</returns>
        public static MvcHtmlString AlbTimeFormat<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
           
            var function = expression.Compile();
            var result = function(helper.ViewData.Model);
            var fieldName = AlbTools.GetFieldName(helper, expression);
            var resHtml = string.Empty;
            if(result==null)
                resHtml= htmlAttributes != null ? helper.TextBox(fieldName, MvcHtmlString.Empty, htmlAttributes).ToString() : helper.TextBox(fieldName, MvcHtmlString.Empty).ToString();
            if (result != null)
            {
                string[] timeRes = result.ToString().Split(':');
                resHtml= htmlAttributes != null ? helper.TextBox(fieldName, timeRes[0] + ":" +
                    timeRes[1], htmlAttributes).ToString() : helper.TextBox(fieldName, timeRes[0] + ":" +
                    timeRes[1]).ToString();
            }
             resHtml= htmlAttributes != null ? helper.TextBox(fieldName, MvcHtmlString.Empty, htmlAttributes).ToString() : helper.TextBox(fieldName, string.Empty).ToString();
            return SetValueToInput<TValue>(result, resHtml);
        }

        /// <summary>
        /// Helper qui permet de crée deux combo "Heures: minites"
        /// </summary>
        /// <typeparam name="TModel">Modele</typeparam>
        /// <typeparam name="TValue">Valeur</typeparam>
        /// <param name="helper">Html helper</param>
        /// <param name="expression">Lambda expression utilisée dans le Html Helper d'origine</param>
        /// <param name="htmlAttributes"> Les attributs Html</param>
        /// <returns>heure formattée</returns>
        public static MvcHtmlString AlbTimePicker<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string htmlId, object htmlAttributes = null, string htmlName = "")
        {
           
            object customHtmlAttributes = htmlAttributes;

            var function = expression.Compile();
            var result = function(helper.ViewData.Model);
            TimeSpan? timeValue = null;
            //var nameToReplace = GetFieldName(helper, expression);
            if (result != null)
            {
                var timeRes = result.ToString().Split(':');
                timeValue = new TimeSpan(int.Parse(timeRes[0]), int.Parse(timeRes[1]), 0);
            }
            var hours = Enumerable.Range(0, 24)
                .Select(i => new SelectListItem
                                 {
                                     Value =
                                         i >= 10
                                             ? i.ToString(CultureInfo.CurrentUICulture)
                                             : "0" + i.ToString(CultureInfo.CurrentUICulture)
                                     ,
                                     Text =
                                         i >= 10
                                             ? i.ToString(CultureInfo.CurrentUICulture)
                                             : "0" + i.ToString(CultureInfo.CurrentUICulture),
                                     Selected = result == null ? false : (timeValue.Value.Hours == i)
                                 }).ToList();
            hours.Insert(0, new SelectListItem { Selected = result == null, Text = string.Empty, Value = string.Empty });
            var minutes = Enumerable.Range(0, 60)
                .Select(i => new SelectListItem
                                 {
                                     Value =
                                         i >= 10
                                             ? i.ToString(CultureInfo.CurrentUICulture)
                                             : "0" + i.ToString(CultureInfo.CurrentUICulture),
                                     Text =
                                         i >= 10
                                             ? i.ToString(CultureInfo.CurrentUICulture)
                                             : "0" + i.ToString(CultureInfo.CurrentUICulture),
                                     Selected = result == null ? false : (timeValue.Value.Minutes == i)
                                 }).ToList();
            minutes.Insert(0, new SelectListItem { Selected = result == null, Text = string.Empty, Value = string.Empty });

            var timeSpanControlHour = helper.DropDownList(htmlId + "Hours", hours, htmlAttributes);
            var timeSpanControlMinutes = " : " + helper.DropDownList(htmlId + "Minutes", minutes, htmlAttributes);

            //var htmlHourControl = ReplaceIdSelect(timeSpanControlHour.ToString(), !string.IsNullOrEmpty(htmlName) ? htmlName : htmlId, "Hours");
            //var htmlMinutesControl = ReplaceIdSelect(timeSpanControlMinutes.ToString(), !string.IsNullOrEmpty(htmlName) ? htmlName : htmlId, "Minutes");
            var htmlHourControl = ReplaceIdSelect(timeSpanControlHour.ToString(), htmlId, "Hours");
            var htmlMinutesControl = ReplaceIdSelect(timeSpanControlMinutes.ToString(), htmlId, "Minutes");

            if (!string.IsNullOrEmpty(htmlName))
            {
                htmlHourControl = htmlHourControl.Replace("name=\"" + htmlId + "Hours\"", "name=\"" + htmlName + "Hours\"");
                htmlMinutesControl = htmlMinutesControl.Replace("name=\"" + htmlId + "Minutes\"", "name=\"" + htmlName + "Minutes\"");
            }

            var nameHidden = !string.IsNullOrEmpty(htmlName) ? htmlName : GetNameSelect(timeSpanControlHour.ToString(), htmlId, "Hours").Replace("Hours", "").Replace("_", ".");

            var htmlHiddenInput = string.Format("<input id=\"{0}\" type=\"hidden\" value=\"{1}\" name=\"{2}\"/>", htmlId, result != null ? result.ToString() : string.Empty, nameHidden);

            return MvcHtmlString.Create(htmlHourControl + htmlMinutesControl + htmlHiddenInput);
        }

        /// <summary>
        /// Helper qui permet de crée deux combo "Heures: minites" dédiée à l'écran des IS
        /// </summary>
        /// <typeparam name="TModel">Modele</typeparam>
        /// <typeparam name="TValue">Valeur</typeparam>
        /// <param name="helper">Html helper</param>
        /// <param name="expression">Lambda expression utilisée dans le Html Helper d'origine</param>
        /// <param name="htmlId">L'id du input hidden</param>
        /// <param name="isDisabled">Détermine si les éléments select sont désactivés</param>
        /// <returns>heure formattée</returns>
        public static MvcHtmlString TimePickerInfosSpe<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string htmlId, bool isDisabled = false) {
            var arrayId = htmlId?.Split('_');
            if (arrayId is null || arrayId.FirstOrDefault() != "map") {
                throw new ArgumentException($"{nameof(htmlId)} must starts with 'map_'", nameof(htmlId));
            }
            var function = expression.Compile();
            var result = function(helper.ViewData.Model) as string;
            TimeSpan? timeValue = null;
            if (result.ContainsChars()) {
                var timeRes = result.ToString().Split(':');
                timeValue = new TimeSpan(int.Parse(timeRes[0]), int.Parse(timeRes[1]), 0);
            }
            var hours = Enumerable.Range(0, 24)
                .Select(i => new SelectListItem {
                    Value = i >= 10 ? i.ToString(CultureInfo.CurrentUICulture) : "0" + i.ToString(CultureInfo.CurrentUICulture),
                    Text = i >= 10 ? i.ToString(CultureInfo.CurrentUICulture) : "0" + i.ToString(CultureInfo.CurrentUICulture),
                    Selected = result.IsEmptyOrNull() ? false : (timeValue.Value.Hours == i)
                }).ToList();
            hours.Insert(0, new SelectListItem { Selected = result.IsEmptyOrNull(), Text = string.Empty, Value = string.Empty });
            var minutes = Enumerable.Range(0, 60)
                .Select(i => new SelectListItem {
                    Value = i >= 10 ? i.ToString(CultureInfo.CurrentUICulture) : "0" + i.ToString(CultureInfo.CurrentUICulture),
                    Text = i >= 10 ? i.ToString(CultureInfo.CurrentUICulture) : "0" + i.ToString(CultureInfo.CurrentUICulture),
                    Selected = result.IsEmptyOrNull() ? false : (timeValue.Value.Minutes == i)
                }).ToList();
            minutes.Insert(0, new SelectListItem { Selected = result.IsEmptyOrNull(), Text = string.Empty, Value = string.Empty });

            const string hr = "Hour";
            const string mn = "Minutes";
            const string hrmnClass = "HourMinute";
            string selectId = string.Join("_", arrayId.Where((s, x) => x > 0));
            string selectHourHtml = $@"
<select id='{selectId}{hr}' name='{selectId}{hr}' class='{hrmnClass}{(isDisabled ? " readonly" : string.Empty)}'{(isDisabled ? " disabled" : string.Empty)}>
{string.Join("\n", hours.Select(x => $"<option value=\"{x.Value}\"{(x.Selected ? " selected" : string.Empty)}>{x.Text}</option>"))}
</select>";
            string selectMinutesHtml = $@"
<select id='{selectId}{mn}' name='{selectId}{mn}' class='{hrmnClass}{(isDisabled ? " readonly" : string.Empty)}'{(isDisabled ? " disabled" : string.Empty)}>
{string.Join("\n", minutes.Select(x => $"<option value=\"{x.Value}\"{(x.Selected ? " selected" : string.Empty)}>{x.Text}</option>"))}
</select>";

            string hiddenHtml = $"<input id='{htmlId}' name='{htmlId}' type='hidden' value=\"{(result.ContainsChars() ? result.ToString() : string.Empty)}\" />";
            string jsHtml = $@"
<script type='text/javascript'>
(function(d) {{
    $(d).on('change', '#{selectId}{hr}, #{selectId}{mn}', function() {{
        let changeHH = $(this).attr('id').indexOf('Hour') > -1;
        let hh = $('#{selectId}{hr}').val();
        let mm = $('#{selectId}{mn}').val();
        if (hh === '' && changeHH) {{
            $('#{htmlId}, #{selectId}{mn}').clear();
        }}
        else if (mm === '' && !changeHH) {{
            $('#{htmlId}, #{selectId}{hr}').clear();
        }}
        else {{
            if (mm === '') {{ mm = '00'; $('#{selectId}{mn}').val('00'); }}
            if (hh === '') {{ hh = '00'; $('#{selectId}{hr}').val('00'); }}
            $('#{htmlId}').val(hh + ':' + mm);
        }}
    }});
}})(document);
</script>";

            return MvcHtmlString.Create(Environment.NewLine + string.Join(
                Environment.NewLine,
                new[] { jsHtml, $"{selectHourHtml} : {selectMinutesHtml}", hiddenHtml }));
        }



        #region private method
        /// <summary>
        /// Affecte la bonne valeur au controle HTML à retourner (input).
        /// Dansle cas d'utilisation de helper, l'ancienne valeur est sauvegardé deans le model state
        /// </summary>
        /// <typeparam name="TValue">valeur a retourner </typeparam>
        /// <param name="result"></param>
        /// <param name="resHtml">resultat html à traiter</param>
        /// <param name="dateFormat">format de la date à utiliser</param>
        /// <returns></returns>
        private static MvcHtmlString SetValueToInput<TValue>( TValue result, string resHtml,string dateFormat="")
        {
            var valHtml = resHtml.ToLower().Split(new[] { "value" }, StringSplitOptions.None);
            if (valHtml != null && valHtml.Count() > 1)
                return new MvcHtmlString(resHtml.Replace(valHtml[1].Replace("=", string.Empty).Replace("/", string.Empty).Replace("\"", string.Empty).Replace(">", string.Empty).Trim(), 
                    string.IsNullOrEmpty(dateFormat)?result.ToString():Convert.ToDateTime(result).ToString(dateFormat)));
            return new MvcHtmlString(resHtml);
        }
        /// <summary>
        /// Remplacer l'ancien Html ID par le nouveau
        /// </summary>
        /// <param name="timeSpanControl">XControl time</param>
        /// <param name="id">Html Id à utiliser</param>
        /// <param name="suffixe">Suffixe</param>
        /// <returns></returns>
        private static string ReplaceIdSelect(string timeSpanControl, string id, string suffixe)
        {
            if (!timeSpanControl.Contains("id"))
            {
                timeSpanControl = timeSpanControl.Replace("<select ", "<select id=\"" + id + suffixe + "\" ");
            }
            else
            {
                var oldId = GetOldIdSelect(timeSpanControl, id, suffixe);
                timeSpanControl = timeSpanControl.Replace(oldId, id + suffixe);
            }

            return timeSpanControl;
        }
        /// <summary>
        /// Retouirne l'ancien ID Html
        /// </summary>
        /// <param name="timeSpanControl">Control timespan</param>
        /// <param name="id">Ancien ID</param>
        /// <param name="suffixe">Suffixe</param>
        /// <returns></returns>
        private static string GetOldIdSelect(string timeSpanControl, string id, string suffixe)
        {
            var frstIndexId = timeSpanControl.IndexOf("id=\"");
            var lstIndxId = timeSpanControl.IndexOf("\"", frstIndexId + 4);
            return timeSpanControl.Substring(frstIndexId + 4, lstIndxId - frstIndexId - 4);
        }

        /// <summary>
        /// Retourne le Html Name utiliser par le modèle
        /// </summary>
        /// <param name="timeSpanControl">Time Span Control</param>
        /// <param name="id">ID à utiliser</param>
        /// <param name="suffixe">Suffixe</param>
        /// <returns></returns>
        private static string GetNameSelect(string timeSpanControl, string id, string suffixe)
        {
            var frstIndexId = timeSpanControl.IndexOf("name=\"");
            var lstIndxId = timeSpanControl.IndexOf("\"", frstIndexId + 6);
            return timeSpanControl.Substring(frstIndexId + 6, lstIndxId - frstIndexId - 6);
        }
        /// <summary>
        /// Test si le type est nullable ou pas
        /// </summary>
        /// <typeparam name="T">Type </typeparam>
        /// <param name="obj">objet type</param>
        /// <returns>True si c'est nullable </returns>
        static bool IsNullable<T>(T obj)
        {
            if (!typeof(T).IsGenericType)
                return false;

            return typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        ///// <summary>
        ///// renvoie la valeur de l'id Html
        ///// </summary>
        ///// <typeparam name="TModel">Model envoyé</typeparam>
        ///// <typeparam name="TValue"></typeparam>
        ///// <param name="helper">Html Helper</param>
        ///// <param name="expression">Lambda expression</param>
        ///// <returns>valeur de l'id Html</returns>
        //private static string GetFieldName<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        //{
        //    var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
        //    var fieldName = ExpressionHelper.GetExpressionText(expression);
        //    return fieldName;
        //}
        #endregion

    }



}