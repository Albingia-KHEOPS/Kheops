using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GestUtilisateurs
{
    [DataContract]
    public class UtilisateurBrancheDto
    {
        [DataMember]
        [Column(Name = "UTILISATEUR")]
        public string Utilisateur { get; set; }

        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }

        [DataMember]
        [Column(Name = "TYPEDROIT")]
        public string TypeDroit { get; set; }
    }
}
