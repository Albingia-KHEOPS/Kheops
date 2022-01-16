using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Clause;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.ChoixClauses
{
    [DataContract]
    public class ClauseVisualisationDto
    {
        [DataMember]
        public List<ClauseDto> Clauses { get; set; }
        [DataMember]
        public RisqueDto Risques { get; set; }
    }
}
