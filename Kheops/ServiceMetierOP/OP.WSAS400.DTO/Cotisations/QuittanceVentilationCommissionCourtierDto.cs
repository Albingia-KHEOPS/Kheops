using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceVentilationCommissionCourtierDto
    {
        [DataMember]
        [Column(Name = "CODETAXE")]
        public string CodeCourtier { get; set; }
        [DataMember]
        [Column(Name = "LIBELLECOURTIER")]
        public string LibelleCourtier { get; set; }
        [DataMember]
        [Column(Name = "REPARTITION")]
        public Double Repartition { get; set; }
        [DataMember]
        [Column(Name = "HORSCATNAT")]
        public Double HorsCATNAT { get; set; }
        [DataMember]
        [Column(Name = "CATNAT")]
        public Double CATNAT { get; set; }
        [DataMember]
        [Column(Name = "TOTAL")]
        public Double Total { get; set; }
    }
}
