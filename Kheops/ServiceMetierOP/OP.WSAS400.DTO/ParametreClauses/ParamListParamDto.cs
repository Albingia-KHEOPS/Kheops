using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamListParamDto : ParamClauses_Base
    {
        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        [DataMember]
        [Column(Name = "PARAM")]
        public string Param { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        [Column(Name = "NUMORDRE")]
        public Int64 NumOrdre { get; set; }
        [Column(Name = "EMPLMODIF")]
        public string EmplacementModif { get; set; }
        [Column(Name="AJOUTCLAUSIER")]
        public string AjtClausier { get; set; }
        [Column(Name="AJOUTLIBRE")]
        public string AjtLibre { get; set; }
        [DataMember]
        [Column(Name = "CLAUSEMOD1")]
        public string ModeleClause1 { get; set; }
        [DataMember]
        [Column(Name = "CLAUSEMOD2")]
        public string ModeleClause2 { get; set; }
        [DataMember]
        [Column(Name = "CLAUSEMOD3")]
        public Int32 ModeleClause3 { get; set; }
        [DataMember]
        [Column(Name = "LIBSCRIPT")]
        public string ScriptControle { get; set; }
        [DataMember]
        [Column(Name = "LIBMODELCLAUSE")]
        public string LibModelClause { get; set; }
        [DataMember]
        [Column(Name = "EMPLACEMENT")]
        public string Emplacement { get; set; }
        [DataMember]
        [Column(Name="LIBEMPLACEMENT")]
        public string LibEmplacement { get; set; }
        [DataMember]
        [Column(Name = "SOUSEMPLACEMENT")]
        public string SousEmplacement { get; set; }
        [Column(Name = "NUMORDO")]
        public Single NumOrdo { get; set; }
        [DataMember]
        public bool AjoutClausier { get; set; }
        [DataMember]
        public bool AjoutLibre { get; set; }
        [DataMember]
        public bool EmplModif { get; set; }
        [DataMember]
        public string NumOrdonnancement { get; set; }
    }
}
