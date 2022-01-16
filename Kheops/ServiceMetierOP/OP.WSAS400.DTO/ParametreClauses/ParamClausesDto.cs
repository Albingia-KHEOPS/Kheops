using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamClausesDto : ParamClauses_Base
    {
        
        [DataMember]
        public int Etape { get; set; }
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public List<ParametreDto> Services { get; set; }
    }
}
