using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.DocumentsJoints
{
    [DataContract]
    public class DocumentsAddDto
    {
        [DataMember]
        [Column(Name = "DOCID")]
        public Int64 DocumentId { get; set; }
        [DataMember]
        [Column(Name = "TYPEDOC")]
        public string TypeDoc { get; set; }
        [DataMember]
        [Column(Name = "TITREDOC")]
        public string Titre { get; set; }
        [DataMember]
        [Column(Name = "FILEDOC")]
        public string Fichier { get; set; }
        [DataMember]
        [Column(Name = "PATHDOC")]
        public string Chemin { get; set; }
        [DataMember]
        [Column(Name = "REFDOC")]
        public string Reference { get; set; }

        [DataMember]
        public List<ParametreDto> TypesDoc { get; set; }
    }
}
