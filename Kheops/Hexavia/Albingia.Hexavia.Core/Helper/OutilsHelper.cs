using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albingia.Hexavia.Core.Helper
{
    public class OutilsHelper
    {
        public static string ToString(object contenu)
        {

            if (contenu != null)
            {
                return contenu.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
