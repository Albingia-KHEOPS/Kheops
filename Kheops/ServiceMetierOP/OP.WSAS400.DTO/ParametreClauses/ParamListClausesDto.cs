using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ParametreClauses
{
    [DataContract]
    public class ParamListClausesDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public Int64 Code { get; set; }
        [DataMember]
        [Column(Name = "NOM1")]
        public string Nom1 { get; set; }
        [DataMember]
        [Column(Name = "NOM2")]
        public string Nom2 { get; set; }
        [DataMember]
        [Column(Name = "NOM3")]
        public int Nom3 { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        [Column(Name = "RATTACH")]
        public Int64 Rattache { get; set; }
        [DataMember]
        [Column(Name = "ORDRE")]
        public Int64 Ordre { get; set; }
        [Column(Name = "NATURE")]
        public string Nature { get; set; }
        [Column(Name="IMPRESSANNEXE")]
        public string ImpressAnnexe { get; set; }
        [Column(Name="CODEANNEXE")]
        public string CodeAnnexe { get; set; }
        [Column(Name="ATTRIBUTIMPRESS")]
        public string AttributImpress { get; set; }
        [Column(Name="VERSION")]
        public int Version { get; set; }
    }
}
