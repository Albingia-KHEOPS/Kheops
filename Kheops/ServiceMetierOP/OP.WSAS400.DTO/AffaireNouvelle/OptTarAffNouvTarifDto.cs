using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class OptTarAffNouvTarifDto
    {
        [DataMember]
        public Int64 CodeTarif { get; set; }
        [DataMember]
        public Int64 GuidTarif { get; set; }
        [DataMember]
        public Double LCIVal { get; set; }
        [DataMember]
        public string LCIUnit { get; set; }
        [DataMember]
        public string LCIType { get; set; }
        [DataMember]
        public Int64 IdLCICpx { get; set; }
        [DataMember]
        public Double FRHVal { get; set; }
        [DataMember]
        public string FRHUnit { get; set; }
        [DataMember]
        public string FRHType { get; set; }
        [DataMember]
        public Int64 IdFRHCpx { get; set; }
        [DataMember]
        public Double ASSVal { get; set; }
        [DataMember]
        public string ASSUnit { get; set; }
        [DataMember]
        public string ASSType { get; set; }
        [DataMember]
        public decimal PRIVal { get; set; }
        [DataMember]
        public string PRIUnit { get; set; }
        [DataMember]
        public string PRIType { get; set; }
        [DataMember]
        public Double PRIMPro { get; set; }
        [DataMember]
        public bool CheckRow { get; set; }
    }
}
