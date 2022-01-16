using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hexavia.Models
{
    [Serializable]
    public class AffaireGeoloc
    {
        [XmlAttribute("Branche")]
        public string Branche { get; set; }

        [XmlAttribute("Ipb")]
        public string NumContrat { get; set; }

        [XmlAttribute("Alx")]
        public int Version { get; set; }

        [XmlAttribute("Reference")]
        public string Reference { get; set; }

        [XmlAttribute("Type")]
        public string Type { get; set; }

        [XmlAttribute("Date")]
        public string DateSaisie { get; set; }


        //public short NumInterneAvenant { get; set; }
        //public string TypeTraitement { get; set; }

    }
}
