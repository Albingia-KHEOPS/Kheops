using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Avenant
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
