using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.NavigationArbre
{
    [DataContract]
    public class OptionDto
    {
        [Column(Name = "KDDRSQ")]
        [DataMember]
        public int Risque { get; set; }

        [Column(Name = "KDDFOR")]
        [DataMember]
        public int Formule { get; set; }

        [Column(Name = "KDDOPT")]
        [DataMember]
        public int Option { get; set; }

        [Column(Name="DESCRIPTION")]
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string TagOption { get; set; }

    }
}
