using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class OptTarAffNouvGaranDto
    {
        [DataMember]
        public Int64 CodeForm { get; set; }
        [DataMember]
        public string CodeGaran { get; set; }
        [DataMember]
        public string DescGaran { get; set; }
        [DataMember]
        public string LettreForm { get; set; }
        [DataMember]
        public List<OptTarAffNouvTarifDto> Tarifs { get; set; }
    }
}
