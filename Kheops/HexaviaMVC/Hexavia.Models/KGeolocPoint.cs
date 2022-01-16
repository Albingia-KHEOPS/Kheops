using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hexavia.Models
{
    [Serializable]
    public class KGeolocPoint
    {
        [XmlElement("Latitude")]
        public double Lat { get; set; }
        [XmlElement("Longitude")]
        public double Lon { get; set; }
    }
}
