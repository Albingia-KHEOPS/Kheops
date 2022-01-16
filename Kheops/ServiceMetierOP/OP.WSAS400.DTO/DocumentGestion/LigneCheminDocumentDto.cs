using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentGestion
{
    public class LigneCheminDocumentDto
    {
        [DataMember]
        [Column(Name = "IDENTIFIANT")]
        public string IdChemin { get; set; }

        [DataMember]
        [Column(Name = "LIBELLE")]
        public string LibelleChemin { get; set; }

        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }
        
        [DataMember]
        [Column(Name = "CHEMIN")]
        public string Chemin { get; set; }

        [DataMember]
        [Column(Name = "SERVEUR")]
        public string Serveur { get; set; }

        [DataMember]
        [Column(Name = "RACINE")]
        public string Racine { get; set; }

        [DataMember]
        [Column(Name = "ENVIRONNEMENT")]
        public string Environnement { get; set; }
    }
}
