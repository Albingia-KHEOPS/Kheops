using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.GestionDocumentContrat
{
    public class LignePieceJointeDto
    {       
        [DataMember]
        [Column(Name = "DESIGNATION")]
        public string LibelleDocument { get; set; }

        [DataMember]
        [Column(Name = "CHEMINDOCUMENT")]
        public string CheminFichier { get; set; }

        [DataMember]
        [Column(Name = "NOMDOCUMENT")]
        public string NomFichier { get; set; }

        [DataMember]     
        public int Taille { get; set; }


    }
}
