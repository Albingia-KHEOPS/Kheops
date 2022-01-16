using System.Runtime.Serialization;

namespace Hexavia.Models.GoogleMaps
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
