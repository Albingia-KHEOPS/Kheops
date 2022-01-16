using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.GoogleMaps
{
    /// <summary>
    /// Viewport
    /// </summary>
    public class Viewport
    {
        // Northeast
       
        public Location northeast { get; set; }
        // Southwest
      
        public Location southwest { get; set; }
    }

}
