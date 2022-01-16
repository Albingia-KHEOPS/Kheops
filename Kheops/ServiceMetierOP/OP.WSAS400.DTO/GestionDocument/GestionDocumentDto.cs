using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.GestionDocument
{
    [DataContract]
    public class GestionDocumentDto
    {
        [DataMember]
        public List<DocumentDto> Distributions { get; set; }
    }
}
