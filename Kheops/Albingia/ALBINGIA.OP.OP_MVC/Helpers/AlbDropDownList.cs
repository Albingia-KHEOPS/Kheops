using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.Framework.Common.Tools;
using System.Linq;

namespace ALBINGIA.OP.OP_MVC.Helpers
{
    public static class AlbDropDownList
    {
        /// <summary>
        /// Helper DropDownList qui gère les ToolTip
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <typeparam name="TProperty">Propriété</typeparam>
        /// <param name="helper">Html Helper</param>
        /// <param name="expression">Lambda expression utilisée dans le Html Helper d'origine</param>
        /// <param name="selectListItems">Liste des élement à traiter de type AlbSelectListItem</param>
        /// <param name="htmlIdDropDownList">Id de la HtmlDropDownList</param>
        /// <param name="optionLabel">Option label</param>
        /// <param name="htmlAttributes">Attributs Html</param>
        /// <param name="generateTitle"> </param>
        /// <param name="genEmptyLine"> </param>
        /// <returns></returns>
        public static MvcHtmlString AlbDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, List<AlbSelectListItem> selectListItems,
          string htmlIdDropDownList, string optionLabel = "", object htmlAttributes = null, bool generateTitle = false, bool genEmptyLine = true, string htmlCssOptionStyle = "", string replaceHtmlName = "")
        {

            var selectedValue = selectListItems.Find(item => item.Selected);
            var emptyItem = selectListItems.FirstOrDefault(item=> string.IsNullOrEmpty(item.Value.Trim()) && string.IsNullOrEmpty(item.Text.Trim()));
            if (emptyItem != null)
                selectListItems.Remove(emptyItem);
            var retHtmlString = helper.DropDownListFor(expression, selectListItems, optionLabel, htmlAttributes).ToHtmlString();
          
            if (!genEmptyLine)
                retHtmlString = retHtmlString.Replace("<option value=\"" + string.Empty + "\"></option>", string.Empty);
            
            string nameToReplace = AlbTools.GetFieldName(helper, expression);

            retHtmlString = !string.IsNullOrEmpty(replaceHtmlName) ? retHtmlString.Replace("name=\"" + nameToReplace + "\"",
                "name=\"" + replaceHtmlName + "\"") : retHtmlString;

            if (selectedValue != null)
            {
                var valSel = selectedValue.Value;

                retHtmlString = retHtmlString.Replace("value=\"" + valSel + "\"",
                                       "value=\"" + valSel + "\"" + " selected=\"selected\"" + " ");
            }
            if (!generateTitle || selectListItems.Count == 0)
            {
                return MvcHtmlString.Create(retHtmlString);
            }
            var resHtmlDrop = string.Empty;
            selectListItems.ForEach(item =>
            {
                resHtmlDrop = retHtmlString.Replace("value=\"" + item.Value + "\"",
                                      "value=\"" + item.Value + "\"" + " title=\"" +
                                      item.Title + "\"" + " style=\"" +
                                      htmlCssOptionStyle + "\"" + " ");
                retHtmlString = resHtmlDrop;
            });


            var divTitles = new StringBuilder();
            divTitles.Append(string.Format("<div id='{0}' style='display:none'>", htmlIdDropDownList));
            foreach (var item in selectListItems)
            {
                divTitles.Append(string.Format("<div id=\"{0}\" albDescriptif=\"{1}\">{2}</div>", htmlIdDropDownList + item.Value, item.Descriptif, item.Title));
            }

            divTitles.Append("</div>");

            var htmlDivTitles = divTitles.ToString();

            return MvcHtmlString.Create(resHtmlDrop + Environment.NewLine + htmlDivTitles);
        }
    }
}

