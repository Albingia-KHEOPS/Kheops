using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationGarantieDto
    {
        [DataMember]
        public string CodeGarantie { get; set; }
        [DataMember]
        public string NomGarantie { get; set; }
        [DataMember]
        public string CodeRisque { get; set; }
        [DataMember]
        public string CodeFormule { get; set; }
        [DataMember]
        public string Tarif { get; set; }
        [DataMember]
        public string LCIValeur { get; set; }
        [DataMember]
        public string LCIUnite { get; set; }
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
        [DataMember]
        public string LienKpgaran { get; set; }
    }
}
