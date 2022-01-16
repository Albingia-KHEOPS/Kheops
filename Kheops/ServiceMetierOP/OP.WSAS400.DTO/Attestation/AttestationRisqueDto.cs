using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Attestation
{
    [DataContract]
    public class AttestationRisqueDto
    {
        [DataMember]
        public Int32 Code { get; set; }
        [DataMember]
        public string CodesObj { get; set; }
        [DataMember]
        public List<AttestationObjetDto> Objets { get; set; }
        [DataMember]
        public List<AttestationFormuleDto> Formules { get; set; }
    }
}
