using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SyntheseDocuments
{
    public class SyntheseDocumentsDocPlatDto
    {
        [Column(Name="LOTID")]
        public Int64 LotId { get; set; }
        [Column(Name = "ORDRE")]
        public Int64 Ordre { get; set; }
        [Column(Name = "TYPEDEST")]
        public string TypeDest { get; set; }
        [Column(Name = "IDDEST")]
        public Int64 IdDest { get; set; }
        [Column(Name = "TYPEENVOI")]
        public string TypeEnvoi { get; set; }
        [Column(Name = "LIBTYPEENVOI")]
        public string LibTypeEnvoi { get; set; }
        [Column(Name = "DOCID")]
        public Int64 DocId { get; set; }
        [Column(Name = "NOMDOC")]
        public string NomDoc { get; set; }
        [Column(Name = "NBEXEMPL")]
        public Int64 NbExempl { get; set; }
        [Column(Name = "IMPRIMABLE")]
        public string Imprimable { get; set; }
    }
}
