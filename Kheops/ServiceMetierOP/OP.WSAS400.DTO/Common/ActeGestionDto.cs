using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Common
{
    public class ActeGestionDto
    {
        [DataMember]
        [Column(Name = "DATEANNEE")]
        public Int16 DateAnnee { get; set; }
        [DataMember]
        [Column(Name = "DATEMOIS")]
        public Int16 DateMois { get; set; }
        [DataMember]
        [Column(Name = "DATEJOUR")]
        public Int16 DateJour { get; set; }
        [DataMember]
        [Column(Name = "HEURE")]
        public Int16 Heure { get; set; }
        [DataMember]
        [Column(Name = "NUMERO")]
        public Int32 Numero { get; set; }
        [DataMember]
        [Column(Name = "TYPETRAITEMENT")]
        public string TypeTraitement { get; set; }
        [DataMember]
        [Column(Name = "LIBELLE")]
        public string Libelle { get; set; }
        [DataMember]
        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }
        [DataMember]
        [Column(Name = "UTILISATEUR")]
        public string Utilisateur { get; set; }
        [DataMember]
        [Column(Name = "CODETRAITEMENT")]
        public string CodeTraitement { get; set; }
        [DataMember]
        [Column(Name = "DATECREATION")]
        public Int64 DateCreationInt { get; set; }
    }
}
