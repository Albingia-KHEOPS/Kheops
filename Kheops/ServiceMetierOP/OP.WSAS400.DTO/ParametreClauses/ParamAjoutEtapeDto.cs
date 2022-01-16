using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamAjoutEtapeDto : ParamClauses_Base
    {
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string LibelleService { get; set; }
        [DataMember]
        public string CodeActGes { get; set; }
        [DataMember]
        public string LibelleActGes { get; set; }
        [DataMember]
        public string Etape { get; set; }
        [DataMember]
        public List<ParametreDto> Etapes { get; set; }
        [DataMember]
        public Int64 NumOrd { get; set; }
    }
}
