using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.OffresRapide
{
    [DataContract]
    public class OffreRapideResultDto
    {
        [DataMember]
        public IList<OffreRapideInfoDto> Offres { get; set; }

        [DataMember]
        public int NbCount { get; set; }
    }
}
