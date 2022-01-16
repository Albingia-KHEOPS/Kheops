
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Hexavia.Console.Infrastructure.Photon
{
    public class PhotonJsonObject
    {  
        public string type { get; set; }    
        public List<Feature> features { get; set; }
    }
}