using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Helpers
{
    public static class AlbLabel
    {
        public static MvcHtmlString AlbLabelFor<TModel, TValue>(
        this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string cssClass = "")
        {
            var retHtmlString = helper.Label(ExpressionHelper.GetExpressionText(expression)).ToHtmlString();
            if (string.IsNullOrEmpty(cssClass))
                return  MvcHtmlString.Create(retHtmlString);
            return MvcHtmlString.Create(retHtmlString.Replace("<label", "<label class=\"" + cssClass + "\" "));
        }
    }
}