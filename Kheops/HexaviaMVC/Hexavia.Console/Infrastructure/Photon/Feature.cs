using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.Photon
{
    public class Feature
    {  
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }
}