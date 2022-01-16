using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.GestionDocument
{
    [DataContract]
    public class DocumentDto
    {
        [DataMember]
        [Column(Name = "CODE")]
        public int CodeDoc { get; set; }
        [DataMember]
        [Column(Name = "CODETXT")]
        public int CodeTxt { get; set; }
        [DataMember]
        [Column(Name="CODEACCOMPAGNANT")]
        public int Accompagnant { get; set; }
        [DataMember]
        [Column(Name="CODEMAIL")]
        public int LienMail { get; set; }
        [DataMember]
        public string NomDoc { get; set; }
        [DataMember]
        public List<GestionDocumentDiffusionDto> Diffusions { get; set; }
    }
}
