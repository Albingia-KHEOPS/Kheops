using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hexavia.Models.GoogleMaps
{
    /// <summary>
    /// Address component 
    /// </summary>
    public class AddressComponent
    {
        //Long name
      
        public string long_name { get; set; }
        //Short name
       
        public string short_name { get; set; }
        // Types of component
     
        public List<string> types { get; set; }
    }

}