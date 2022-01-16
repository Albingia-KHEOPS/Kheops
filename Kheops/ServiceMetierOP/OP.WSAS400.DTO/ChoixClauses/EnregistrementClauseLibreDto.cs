using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.ChoixClauses
{
    [DataContract]
    public class EnregistrementClauseLibreDto
    {
        [DataMember]
        public string retourMsg { get; set; }
        [DataMember]
        public ChoixClausesInfoDto ChoixClausesInfo { get; set; }
    }
}
