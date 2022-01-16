
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.GoogleMaps
{

    /// <summary>
    ///  Geocoding geometry
    /// </summary>
    public class Geometry
    {
        //geocoded latitude,longitude value
        public Location location { get; set; }

        //location type  
        public string location_type { get; set; }

        //recommended viewport 
        public Viewport viewport { get; set; }
    }
}
