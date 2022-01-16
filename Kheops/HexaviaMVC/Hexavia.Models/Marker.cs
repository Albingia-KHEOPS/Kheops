using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hexavia.Models
{
    [Serializable]
    public class Marker
    {
        [XmlElement("Point")]
        public KGeolocPoint Point { get; set; }

        [XmlElement("Gestionnaire")]
        public string Gestionnaire { get; set; }

        [XmlElement("ActeGestion")]
        public AffaireGeoloc ActeGestion { get; set; }
    }
}
