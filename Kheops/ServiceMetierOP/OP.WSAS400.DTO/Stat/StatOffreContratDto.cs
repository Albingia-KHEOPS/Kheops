using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Stat
{
    [DataContract]
    public class StatOffreContratDto
    {
        [DataMember]
        [Column(Name = "NUMERO")]
        public string Num { get; set; }

        [DataMember]
        [Column(Name = "VERSION")]
        public int Version { get; set; }

        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "REFERENCE")]
        public string Reference { get; set; }

        [DataMember]
        [Column(Name = "GESTIONNAIRE")]
        public string Gestionnaire { get; set; }

        [DataMember]
        [Column(Name = "SOUSCRIPTEUR")]
        public string Souscripteur { get; set; }

        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }

        [DataMember]
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }
    }
}
