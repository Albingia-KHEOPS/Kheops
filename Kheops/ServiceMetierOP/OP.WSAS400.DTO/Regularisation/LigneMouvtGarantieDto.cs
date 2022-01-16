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
    public class LigneMouvtGarantieDto : IRCFRGroupItem
    {
        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [DataMember]
        [Column(Name = "SITUATION")]
        public string Situation { get; set; }
        [DataMember]
        [Column(Name = "DATEDEB")]
        public Int64 PeriodeRegulDeb { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public Int64 PeriodeRegulFin { get; set; }
        [DataMember]
        [Column(Name = "ASSIETTE")]
        public double AssietteValeur { get; set; }
        [DataMember]
        [Column(Name = "TAUXFORFAITHTVALEUR")]
        public double TauxForfaitHTValeur { get; set; }
        [DataMember]
        [Column(Name = "TAUXFORFAITHTUNITE")]
        public string TauxForfaitHTUnite { get; set; }
        [DataMember]
        [Column(Name = "MONTANTREGULHF")]
        public double  MontantRegulHF { get; set; }
        [DataMember]
        [Column(Name = "SITUATION")]
        public string  situation { get; set; }

        [DataMember]
        [Column(Name = "IDRCFR")]
        public long IdRCFR { get; set; }
    }
}
