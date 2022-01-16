using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamActeGestionEtapeDto : ParamClauses_Base
    {
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public string CodeActeGestion { get; set; }
        [DataMember]
        public string ActeGestion { get; set; }
        [DataMember]
        public List<ParamListParamDto> ListEtapes { get; set; }
    }
}
