using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Ecran.DetailsObjetRisque
{
    [DataContract]
     public class ActiviteDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "LIBCOURT")]
        public string LibelleCourt { get; set; }
        [DataMember]
        [Column(Name = "LIBLONG")]
        public string LibelleLong { get; set; }
        [DataMember]
        [Column(Name = "CLASSRSQ")]
        public string ClassRsq { get; set; }
        [DataMember]
        [Column(Name = "CLASSTARIF")]
        public string ClassTarif { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "NOMACTIVITE")]
        public string NomActivite { get; set; }
        [DataMember]
        [Column(Name = "CONCEPT")]
        public string Concept { get; set; }
        [DataMember]
        [Column(Name = "FAMILLE")]
        public string Famille { get; set; }
    }
}
