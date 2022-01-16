using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Clause;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.ChoixClauses
{
    [DataContract]
    public class ChoixClausesInfoDto
    {
        [DataMember]
        public List<ParametreDto> Etapes { get; set; }
        [DataMember]
        public List<ClauseDto> Clauses { get; set; }
        [DataMember]
        public List<ParametreDto> ContextesCibles { get; set; }
        //SAB24042016: Pagination clause
        //[DataMember]
        //public List<ClausesContextDto> ContextesCLauses { get; set; }
        //[DataMember]
        //public List<KeyValuePair<string, string>> filtresByContext { get; set; }
    }
}
