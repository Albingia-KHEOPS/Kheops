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
    public class ParamRattachClauseDto : ParamClauses_Base
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string Service { get; set; }
        [DataMember]
        public string CodeActeGestion { get; set; }
        [DataMember]
        public string ActeGestion { get; set; }
        [DataMember]
        public string CodeEtape { get; set; }
        [DataMember]
        public string Etape { get; set; }
        [DataMember]
        public string CodeContexte { get; set; }
        [DataMember]
        public string Contexte { get; set; }
        [DataMember]
        public string CodeEGDI { get; set; }
        [DataMember]
        public string LibelleEGDI { get; set; }
        [DataMember]
        public bool EG { get; set; }
        [DataMember]
        public bool DI { get; set; }
        [DataMember]
        public string Restriction { get; set; }
        [DataMember]
        public string Affichage { get; set; }
        [DataMember]
        public List<ParametreDto> Affichages { get; set; }
        [DataMember]
        public List<ParamListClausesDto> Clauses { get; set; }
    }
}
