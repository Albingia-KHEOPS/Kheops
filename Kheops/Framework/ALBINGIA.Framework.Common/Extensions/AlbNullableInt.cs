using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALBINGIA.Framework.Common.Extensions
{
   public  class AlbNullableInt
    {
       public static bool TryParse(string param, out int? result)
       {
           int iParam;
           bool ret;
          ret= int.TryParse(param, out iParam);
           if (iParam == 0)
               result = null;
           else
               result = iParam;
           return ret;
       }
    }
}
