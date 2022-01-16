using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Quittance
{
    [DataContract]
    public class QuittanceForceTotalDto
    {
        [Column(Name="MONTANTHT")]
        [DataMember]
        public double MontantCalculeHT { get; set; }
        [Column(Name="MONTANTTTC")]
        [DataMember]
        public double MontantCalculeTTC { get; set; }
        [Column(Name="FRAISACC")]
        public double FraisAccessoires { get; set; }
        [Column(Name="TAXEACC")]
        public double TaxeAccessoires { get; set; }
        [Column(Name="TAXEATT")]
        public double TaxeAttentat { get; set; }
    }
}
