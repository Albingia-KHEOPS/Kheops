using System.Globalization;

namespace Hexavia.Tools.Helpers
{
    /// <summary>
    /// OutilsHelper
    /// </summary>
    public class OutilsHelper
    {
        /// <summary>
        /// ToString
        /// </summary>
        /// <param name="contenu"></param>
        /// <returns></returns>
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

        public static double ToDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            } 
            value = value.Replace(',', '.');
            double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double convertedValue);
            return convertedValue;
        }     
    }
}
