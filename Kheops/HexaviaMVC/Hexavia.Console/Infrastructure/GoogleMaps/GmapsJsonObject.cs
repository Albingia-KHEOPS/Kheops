using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.GoogleMaps
{
    /// <summary>
    /// GmapsJsonObject : google maps geocoding response structure
    /// </summary>
    public class GmapsJsonObject
    {
        // error message if exist
       
        public string error_message { get; set; }
        // Address list
       
        public List<GmapsResult> results { get; set; }
        // response status : "OK" ,"REQUEST_DENIED"
     
        public string status { get; set; }
    }
}
