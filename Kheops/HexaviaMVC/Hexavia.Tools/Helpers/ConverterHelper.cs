using Newtonsoft.Json;
using System.Globalization;
using System.Linq;

namespace Hexavia.Tools.Helpers
{
    /// <summary>
    /// ConverterHelper
    /// </summary>
    public class ConverterHelper
    {
        /// <summary>
        /// ConcatJsonArray : Conversion of json array to single string.
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        public static string ConcatExactJsonArray(string jsonArray , string separator)
        {
            return string.Join(separator, JsonConvert.DeserializeObject<string[]>(jsonArray).Select(x=>x.Trim()));
        }

        //Deserialize a JSON Object
        public static T DeserializeObject<T>(string value)
        {
           return  JsonConvert.DeserializeObject<T>(value);
        }

    }
}
