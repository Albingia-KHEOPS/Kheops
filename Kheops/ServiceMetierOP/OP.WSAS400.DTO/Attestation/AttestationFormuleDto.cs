using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Attestation
{
    [DataContract]
    public class AttestationFormuleDto
    {
        [DataMember]
        public string LettreFormule { get; set; }
        [DataMember]
        public string LibFormule { get; set; }
        [DataMember]    
        public List<AttestationObjetDto> Objets { get; set; }
        [DataMember]
        public List<AttestationGarantieNiv1Dto> Garanties { get; set; }
    }
}
