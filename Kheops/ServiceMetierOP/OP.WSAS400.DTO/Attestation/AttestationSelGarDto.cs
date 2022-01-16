using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Attestation
{
    [DataContract]
    public class AttestationSelGarDto
    {
        [DataMember]
        public string LotId { get; set; }
        [DataMember]
        public string Erreur { get; set; }
        [DataMember]
        public List<AttestationRisqueDto> Risques { get; set; }
    }
}
