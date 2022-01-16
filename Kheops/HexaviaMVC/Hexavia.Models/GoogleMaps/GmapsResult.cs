using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hexavia.Models.GoogleMaps
{
    /// <summary>
    /// Address list of geocoding
    /// </summary>
    public class GmapsResult
    {
        // components of address
        
        public List<AddressComponent> address_components { get; set; }
        // Human-readable address of this location
      
        public string formatted_address { get; set; }
        // geometry Geolocation
       
        public Geometry geometry { get; set; }
        // Unique identifier of a place
       
        public string place_id { get; set; }
       
        public PlusCode plus_code { get; set; }
        // Address type 
  
        public List<string> types { get; set; }
    }
}
