using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.GoogleMaps
{
    /// <summary>
    /// PlusCode
    /// </summary>
    public class PlusCode
    {
        //Compound code
       
        public string compound_code { get; set; }
        //Global code
    
        public string global_code { get; set; }
    }

}
