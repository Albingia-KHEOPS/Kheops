using System;

namespace Hexavia.Tools.Helpers
{
    /// <summary>
    /// IntOutilsHelper
    /// </summary>
    public class IntOutilsHelper
    {
        /// <summary>
        /// ToNullableInt
        /// </summary>
        /// <param name="entier"></param>
        /// <returns></returns>
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
