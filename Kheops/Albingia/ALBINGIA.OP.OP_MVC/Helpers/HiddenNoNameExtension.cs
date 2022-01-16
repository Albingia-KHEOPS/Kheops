using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ALBINGIA.OP.OP_MVC.Helpers {
    public static class HiddenNoNameExtension {
        public static MvcHtmlString HiddenNoName(this HtmlHelper htmlHelper, string id, object value, object htmlAttributes = null) {
            if (htmlAttributes is null) {
                return HiddenNoName(htmlHelper, id, value, new Dictionary<string, object>());
            }
            else {
                return HiddenNoName(htmlHelper, id, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            }
        }
        public static MvcHtmlString HiddenNoName(this HtmlHelper htmlHelper, string id, object value, IDictionary<string, object> htmlAttributes) {
            TagBuilder tagBuilder = new TagBuilder("input");
            if (id.ContainsChars()) {
                tagBuilder.Attributes.Add("id", id);
            }
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
            if (value != null) {
                string text = Convert.ToString(value); //htmlHelper.FormatValue(value, null);
                tagBuilder.MergeAttribute("value", text);
            }
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}