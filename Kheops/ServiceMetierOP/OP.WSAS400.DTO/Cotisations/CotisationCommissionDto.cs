using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationCommissionDto
    {
        [DataMember]
        public string TauxStd { get; set; }
        [DataMember]
        public decimal MontantStd { get; set; }
        [DataMember]
        public string TauxForce { get; set; }
        [DataMember]
        public decimal MontantForce { get; set; }
        [DataMember]
        public string TauxStdCatNat { get; set; }
        [DataMember]
        public decimal MontantStdCatNat { get; set; }
        [DataMember]
        public string TauxForceCatNat { get; set; }
        [DataMember]
        public decimal MontantForceCatNat { get; set; }
    }
}
