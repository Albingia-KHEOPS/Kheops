using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Cotisations
{
    [DataContract]
    public class CotisationGarantieExecListDto
    {
        [DataMember]
        [Column(Name = "CODEGARANTIE")]
        public string CodeGarantie { get; set; }
        [DataMember]
        [Column(Name = "LIBGARANTIE")]
        public string NomGarantie { get; set; }
        [DataMember]
        [Column(Name = "CODERISQUE")]
        public Int32 CodeRisque { get; set; }
        [DataMember]
        [Column(Name = "CODEFORMULE")]
        public string CodeFormule { get; set; }
        [DataMember]
        [Column(Name = "TARIF")]
        public Int32 Tarif { get; set; }
        [DataMember]
        [Column(Name = "VALEURLCI")]
        public Double LCIValeur { get; set; }
        [DataMember]
        [Column(Name = "UNITELCI")]
        public string LCIUnite { get; set; }
        [DataMember]
        [Column(Name = "VALEURASSIETTE")]
        public Double AssietteValeur { get; set; }
        [DataMember]
        [Column(Name = "UNITEASSIETTE")]
        public string AssietteUnite { get; set; }
        [DataMember]
        [Column(Name = "VALEURTAUX")]
        public decimal TauxValeur { get; set; }
        [DataMember]
        [Column(Name = "UNITETAUX")]
        public string TauxUnite { get; set; }
        [DataMember]
        [Column(Name = "COTISHT")]
        public Double CotisationHT { get; set; }
        [DataMember]
        [Column(Name = "COTISTAXE")]
        public Double CotisationTaxe { get; set; }
        [DataMember]
        [Column(Name = "COTISTTC")]
        public Double CotisationTTC { get; set; }
        [DataMember]
        [Column(Name = "LIENKPGARAN")]
        public Int64 LienKpgaran { get; set; }
    }
}
