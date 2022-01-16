using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.GoogleMaps
{
    /// <summary>
    /// Location geocoding
    /// </summary>
    public class Location
    {
        //Latitude
        
        public double lat { get; set; }
        //Longitude
    
        public double lng { get; set; }
    }

}
