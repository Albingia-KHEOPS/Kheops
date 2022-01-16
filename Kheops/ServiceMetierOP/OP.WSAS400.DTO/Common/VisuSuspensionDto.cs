using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Common
{
    [DataContract]
    public class VisuSuspensionDto
    {
        [DataMember]
        [Column(Name = "SUSPVLIST")]
        public List<VisuListSuspensionDto> Suspensions { get; set; }

        [DataMember]
        [Column(Name = "INFOSCONTRAT")]
        public VisuInfosContratDto InfosContrat { get; set; }
    }
}
