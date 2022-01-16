using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamAjoutContexteDto : ParamClauses_Base
    {
        [DataMember]
        public Int64 IdContexte { get; set; }
        [DataMember]
        public string CodeService { get; set; }
        [DataMember]
        public string LibelleService { get; set; }
        [DataMember]
        public string CodeActGes { get; set; }
        [DataMember]
        public string LibelleActGes { get; set; }
        [DataMember]
        public string CodeEtape { get; set; }
        [DataMember]
        public string LibelleEtape { get; set; }
        [DataMember]
        public string CodeContexte { get; set; }
        [DataMember]
        public string LibelleContexte { get; set; }
        [DataMember]
        public string Contexte { get; set; }
        [DataMember]
        public List<ParametreDto> Contextes { get; set; }
        [DataMember]
        public string Specificite { get; set; }
        [DataMember]
        public List<ParametreDto> Specificites { get; set; }
        [DataMember]
        public string CodeSpecificite { get; set; }
        [DataMember]
        public string LibelleSpecificite { get; set; }
        [DataMember]
        public bool AjoutClausier { get; set; }
        [DataMember]
        public bool AjoutLibre { get; set; }
        [DataMember]
        public string ScriptControle { get; set; }
        [DataMember]
        public List<ParametreDto> ScriptsControle { get; set; }
        [DataMember]
        public string ModeleClause1 { get; set; }
        [DataMember]
        public List<ParametreDto> ModelesClause1 { get; set; }
        [DataMember]
        public string ModeleClause2 { get; set; }
        [DataMember]
        public List<ParametreDto> ModelesClause2 { get; set; }
        [DataMember]
        public string ModeleClause3 { get; set; }
        [DataMember]
        public List<ParametreDto> ModelesClause3 { get; set; }
        [DataMember]
        public bool EmplacementModif { get; set; }
        [DataMember]
        public string Emplacement { get; set; }
        [DataMember]
        public List<ParametreDto> Emplacements { get; set; }
        [DataMember]
        public string SousEmplacement { get; set; }
        [DataMember]
        public List<ParametreDto> SousEmplacements { get; set; }
        [DataMember]
        public string NumOrdonnancement { get; set; }
        [DataMember]
        public List<ParametreDto> NumOrdonnancements { get; set; }
    }
}
