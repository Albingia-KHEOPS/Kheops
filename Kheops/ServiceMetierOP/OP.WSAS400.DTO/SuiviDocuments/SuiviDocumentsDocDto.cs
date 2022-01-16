using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.SuiviDocuments
{
    [DataContract]
    public class SuiviDocumentsDocDto
    {
        [DataMember]
        public Int64 DocId { get; set; }
        [DataMember]
        public string TypeDoc { get; set; }
        [DataMember]
        public string CodeDoc { get; set; }
        [DataMember]
        public string TypeDocLib { get; set; }
        [DataMember]
        public string NomDoc { get; set; }
        [DataMember]
        public string CheminDoc { get; set; }
        [DataMember]
        public Double EmptyLine { get; set; }
    }
}
