using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.ParametreCibles
{
    public class ParamCiblesDto
    {
        //readonly?        

        [DataMember]
        [Column(Name = "GUIDID")]
        public Int64 GuidId { get; set; }
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        [Column(Name="CODEGRILLE")]
        public string Grille { get; set; }
        [DataMember]
        [Column(Name="LIBGRILLE")]
        public string LibGrille { get; set; }

        [DataMember]
        [Column(Name = "CODECONCEPT")]
        public string CodeConcept { get; set; }
        [DataMember]
        [Column(Name = "CODEFAMILLE")]
        public string CodeFamilleRecherche { get; set; }


        [DataMember]
        [Column(Name = "DATECREA")]
        public Int32 DateCreation { get; set; }

  
    }
}
