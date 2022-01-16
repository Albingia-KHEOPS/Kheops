using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres
{
    [DataContract]
    public class OffreRecupDto
    {
        [DataMember]
        [Column(Name = "TYPE")]
        public string Type { get; set; }

        [DataMember]
        [Column(Name = "BRANCHE")]
        public string Branche { get; set; }

        [DataMember]
        [Column(Name = "CIBLE")]
        public string Cible { get; set; }

        [DataMember]
        [Column(Name = "CIBLELABEL")]
        public string CibleLabel { get; set; }

        [DataMember]
        public string Erreur { get; set; }

        [DataMember]
        public bool MultiObj { get; set; }

    }
}
