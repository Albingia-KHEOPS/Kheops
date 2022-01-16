using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Avenant;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Attestation
{
    [DataContract]
    public class AttestationDto
    {
        [DataMember]
        public List<AvenantAlerteDto> Alertes { get; set; }
        [DataMember]
        public List<ParametreDto> TypesContrat { get; set; }
        [DataMember]
        public List<ParametreDto> TypesAttes { get; set; }
    }
}
