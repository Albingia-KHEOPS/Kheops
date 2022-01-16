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
    public class InfoGarantiesRsqDto
    {
        [DataMember]
        [Column(Name = "DATEDEB")]
        public int ? PeriodeRegulDeb { get; set; }
        [DataMember]
        [Column(Name = "DATEFIN")]
        public int? PeriodeRegulFin { get; set; }
        [DataMember]
        [Column(Name = "CODERSQ")]
        public int Codersq { get; set; }
        [DataMember]
        [Column(Name = "CODEGAR")]
        public Int64  CodGar{ get; set; }

        [DataMember]
        [Column(Name = "DATEDEBP")]
        public int? PeriodDeb { get; set; }
        [DataMember]
        [Column(Name = "DATEFINP")]
        public int? PeriodeFin { get; set; }
        [DataMember]
        [Column(Name = "CODERSQP")]
        public int Codersqp { get; set; }
        [DataMember]
        [Column(Name = "CODEGARP")]
        public Int64 CodGarp { get; set; }
        [DataMember]
        [Column(Name = "SITUATION")]
        public string situation { get; set; }

     
        
    }
}
