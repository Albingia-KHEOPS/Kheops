using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;

namespace ALBINGIA.Framework.Common.Extensions
{
    public static class HtmlHelper
    {
        //http://stackoverflow.com/questions/2088139/how-to-format-the-value-in-a-strongy-typed-view-when-using-asp-net-mvcs-html-te
        //public static MvcHtmlString DateBoxFor<TEntity>(
        //            this HtmlHelper helper,
        //            TEntity model,
        //            Expression<Func<TEntity, DateTime?>> property,
        //            object htmlAttributes)
        //{
        //    DateTime? date = property.Compile().Invoke(model);

        //    // Here you can format value as you wish
        //    var value = date.HasValue ? date.Value.ToShortDateString() : string.Empty;
        //    var name = ExpressionParseHelper.GetPropertyPath(property);

        //    return helper.TextBox(name, value, htmlAttributes);
        //}

        public static MvcHtmlString CustomTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes
        )
        {
            var member = expression.Body as MemberExpression;
            var stringLength = member.Member
                .GetCustomAttributes(typeof(StringLengthAttribute), false)
                .FirstOrDefault() as StringLengthAttribute;

            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            if (stringLength != null)
            {
                attributes.Add("maxlength", stringLength.MaximumLength);
            }
            return htmlHelper.CustomTextBoxFor(expression, attributes);
        } 
    
    }
}
