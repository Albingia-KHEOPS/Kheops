using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace OP.WSAS400.DataLogic
{
    internal class _SelectQueryFields_Tools<T> where T : _SelectQueryFields_Base
    {
        public static string SelectFields()
        {
            string toReturn;
            FieldInfo[] fields = typeof(T).GetFields();
            toReturn = string.Join(",", fields.Select(f => f.Name).ToArray());
            return toReturn;
        }
    }
}