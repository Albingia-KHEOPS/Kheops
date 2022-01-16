using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ALBINGIA.Framework.Common.Extensions
{
    public static class PropertyPath
    {
        public static string GetPropertyPath<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            Match match = Regex.Match(property.ToString(), @"^[^\.]+\.([^\(\)]+)$");
            return match.Groups[1].Value;
        }
    }
}
