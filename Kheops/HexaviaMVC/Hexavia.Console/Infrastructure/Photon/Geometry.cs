using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.Photon
{
    public class Geometry
    {
        public List<double> coordinates { get; set; }     
        public string type { get; set; }
    }

}