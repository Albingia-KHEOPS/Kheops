using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Personnes
{
    [DataContract]
    public class UtilisateurDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }
        [DataMember]
        [Column(Name = "NOM")]
        public string Nom { get; set; }
        [DataMember]
        [Column(Name = "PRENOM")]
        public string Prenom { get; set; }
    }
}
