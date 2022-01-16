using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Offres.Risque
{
    [DataContract]
    public class OrganismeOppDto
    {
        [DataMember]
        [Column(Name = "CODEORGANISME")]
        public int Code { get; set; }

        [DataMember]
        [Column(Name = "ORGANISME")]
        public string Nom { get; set; }

        [DataMember]
        [Column(Name = "CPORGANISME")]
        public string CP { get; set; }

        [DataMember]
        [Column(Name = "VILLEORGANISME")]
        public string Ville { get; set; }

        [DataMember]
        [Column(Name = "PAYSORGANISME")]
        public string Pays { get; set; }
        [DataMember]
        [Column(Name = "NOMPAYS")]
        public string NomPays { get; set; }
        [DataMember]
        [Column(Name = "ADRESSE1")]
        public string Adresse1 { get; set; }
        [DataMember]
        [Column(Name = "ADRESSE2")]
        public string Adresse2 { get; set; }
    }
}
