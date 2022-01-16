using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentsJoints
{
    [DataContract]
    public class DocumentsDto
    {
        [Column(Name = "DOCID")]
        [DataMember]
        public Int64 DocumentId { get; set; }
        [Column(Name = "DATEAJOUT")]
        public int DateAjt { get; set; }
        [Column(Name = "ACTECODE")]
        [DataMember]
        public string ActeCode { get; set; }
        [Column(Name = "ACTELIB")]
        [DataMember]
        public string ActeLib { get; set; }
        [Column(Name = "TITRE")]
        [DataMember]
        public string TitreDocument { get; set; }
        [Column(Name = "FICHIER")]
        [DataMember]
        public string NomFichier { get; set; }
        [Column(Name = "REFERENCE")]
        [DataMember]
        public string Reference { get; set; }
        [Column(Name = "REFERENCECP")]
        public Int64 RefCp { get; set; }
        [Column(Name = "CHEMIN")]
        [DataMember]
        public string Chemin { get; set; }
        [Column(Name = "CODEAVN")]
        public Int64 CodeAvn { get; set; }
        [Column(Name = "CODEAVNDOC")]
        [DataMember]
        public Int64 CodeAvnDoc { get; set; }
        [Column(Name = "DATEAVNCUR")]
        public Int64 DateAvnCur { get; set; }
        [Column(Name="DATEAVNHIST")]
        public Int64 DateAvnHist { get; set; }


        [DataMember]
        public DateTime? DateAjout { get; set; }
        [DataMember]
        public DateTime? DateAvn { get; set; }
        [DataMember]
        public bool ReferenceCP { get; set; }
        [DataMember]
        public bool IsReadOnly { get; set; }
    }
}
