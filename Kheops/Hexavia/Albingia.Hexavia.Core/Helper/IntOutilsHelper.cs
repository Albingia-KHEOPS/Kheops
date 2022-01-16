using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albingia.Hexavia.Core.Helper
{
    public class IntOutilsHelper
    {
        public static int? ToNullableInt(string entier)
        {
            int result;
            if (Int32.TryParse(entier, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
