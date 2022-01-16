using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hexavia.Models
{
    [Serializable]
    [XmlRoot("MarkerList")]
    public class FileGeoloc
    {
        [XmlElement("Marker")]
        public List<Marker> Markers { get; set; }
    }
}
