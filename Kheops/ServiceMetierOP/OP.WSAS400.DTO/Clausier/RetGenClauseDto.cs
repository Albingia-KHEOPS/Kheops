using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Clausier
{
    [DataContract]
    public class RetGenClauseDto
    {
        [DataMember]
        public int NbClausesGenerees { get; set; }
        [DataMember]
        public int NbClausesObligatoires { get; set; }
        [DataMember]
        public int NbClausesSuggerees { get; set; }
        [DataMember]
        public string MsgErreur { get; set; }
        [DataMember]
        public List<ClauseOpChoixDto> ListChoixClauses { get; set; }

    }
}
