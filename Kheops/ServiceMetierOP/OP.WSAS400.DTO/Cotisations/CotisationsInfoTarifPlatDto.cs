using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class CotisationsInfoTarifPlatDto
    {
        [DataMember]
        [Column(Name = "NOMGARANTIE")]
        public string NomGarantie { get; set; }

        [DataMember]
        [Column(Name = "TARIF")]
        public Int32 CodeTarif { get; set; }
        [DataMember]
        [Column(Name = "LCIVALEUR")]
        public Double LCIValeur { get; set; }
        [DataMember]
        [Column(Name = "LCIUNITE")]
        public string LCIUnite { get; set; }
        [DataMember]
        [Column(Name = "FRANCHISEVALEUR")]
        public Double FranchiseValeur { get; set; }
        [DataMember]
        [Column(Name = "FRANCHISEUNITE")]
        public string FranchiseUnite { get; set; }
        [DataMember]
        [Column(Name = "PRIMEVALEUR")]
        public decimal TauxValeur { get; set; }
        [DataMember]
        [Column(Name = "PRIMEUNITE")]
        public string TauxUnite { get; set; }
    }
}
