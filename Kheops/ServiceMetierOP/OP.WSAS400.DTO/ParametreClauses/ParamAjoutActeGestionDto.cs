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
    public class ParamAjoutActeGestionDto : ParamClauses_Base
    {
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string LibelleService { get; set; }
        [DataMember]
        public string ActeGestion { get; set; }
        [DataMember]
        public List<ParametreDto> ActesGestion { get; set; }

    }
}
