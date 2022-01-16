using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.Attestation
{
    
    [DataContract]
    public class AttestationSelRsqDto
    {
        [DataMember]
        public string LotId { get; set; }
        [DataMember]
        public string Erreur { get; set; }
        [DataMember]
        public List<RisqueDto> Risques { get; set; }
    }
}
