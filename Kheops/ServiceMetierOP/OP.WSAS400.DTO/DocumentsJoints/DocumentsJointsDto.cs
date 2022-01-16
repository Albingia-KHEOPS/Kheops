using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.DocumentsJoints
{
    [DataContract]
    public class DocumentsJointsDto
    {
        [DataMember]
        public bool ReadOnly { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string CodeOffre { get; set; }

        [DataMember]
        public List<DocumentsDto> ListDocuments { get; set; }

        [DataMember]
        public bool IsValide { get; set; }
    }
}
