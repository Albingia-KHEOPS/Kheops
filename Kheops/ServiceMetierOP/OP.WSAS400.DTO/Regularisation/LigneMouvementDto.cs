using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class LigneMouvementDto
    {
        [DataMember]
        [Column(Name = "DATEDEB")]
        public Int64 MouvementPeriodeDeb { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public Int64 MouvementPeriodeFin { get; set; }
        [DataMember]
        [Column(Name = "ASSIETTE")]
        public double AssietteValeurMvt { get; set; }
        [DataMember]
        [Column(Name = "TAUX")]
        public Double Taux { get; set; }
        [DataMember]
        [Column(Name = "UNITE")]
        public string Unite { get; set; }
       
    }
}
