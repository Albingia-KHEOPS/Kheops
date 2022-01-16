using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ALBINGIA.Framework.Common.Tools
{
    public class AlbTools
    {
        /// <summary>
        /// renvoie la valeur de l'id Html
        /// </summary>
        /// <typeparam name="TModel">Model envoyé</typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper">Html Helper</param>
        /// <param name="expression">Lambda expression</param>
        /// <returns>valeur de l'id Html</returns>
        public static string GetFieldName<TModel, TValue>(HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            //var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            return fieldName;
        }


      public static bool IsEmptyList<T>(List<T> list)
      {
        var elem = list.FirstOrDefault();
        //T defaultTypeValue = GetDefaultValue<T>(list.FirstOrDefault());
        return Equals(default(T),list.FirstOrDefault());
      }

      public static T GetDefaultValue<T>(T value)
      {
        return default(T);
      }
    }
}
