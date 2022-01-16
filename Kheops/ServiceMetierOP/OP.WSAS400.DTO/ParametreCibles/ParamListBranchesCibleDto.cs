using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ParametreCibles
{
    public class ParamListCibleBranchesDto
    {
        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }

        [DataMember]
        [Column(Name = "CODBRCH")]
        public string CodeBranche { get; set; }
        [DataMember]
        [Column(Name = "LIBBRCH")]
        public string LibelleBranche { get; set; }

        [DataMember]
        [Column(Name = "CODSBRCH")]
        public string CodeSousBranche { get; set; }
        [DataMember]
        [Column(Name = "LIBSBRCH")]
        public string LibelleSousBranche { get; set; }

        [DataMember]
        [Column(Name = "CODCAT")]
        public string CodeCategorie { get; set; }
        [DataMember]
        [Column(Name = "LIBCAT")]
        public string LibelleCategorie { get; set; }
        [DataMember]
        [Column(Name="TEMPLATE")]
        public string Template { get; set; }
    }
}
