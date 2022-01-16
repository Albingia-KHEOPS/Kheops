using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationCanatGareatDto
    {
        [DataMember]
        public string AssietteValeur { get; set; }
        [DataMember]
        public string AssietteUnite { get; set; }
        [DataMember]
        public string TauxValeur { get; set; }
        [DataMember]
        public string TauxUnite { get; set; }
        [DataMember]
        public string CotisationHT { get; set; }
        [DataMember]
        public string CotisationTaxe { get; set; }
        [DataMember]
        public string CotisationTTC { get; set; }
    }
}
