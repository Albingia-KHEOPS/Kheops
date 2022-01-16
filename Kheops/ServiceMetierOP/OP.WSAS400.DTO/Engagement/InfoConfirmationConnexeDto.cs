using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OP.WSAS400.DTO.AffaireNouvelle;

namespace OP.WSAS400.DTO.Engagement
{
    [DataContract]
    public class InfoConfirmationConnexeDto
    {
        [DataMember]
        public List<ContratConnexeDto> ContratsConnexesActuels { get; set; }
        [DataMember]
        public string NumConnexiteOrigine { get; set; }
        [DataMember]
        public List<ContratConnexeDto> ContratsConnexesOrigines { get; set; }
        [DataMember]
        public ContratDto ContratOrigine { get; set; }
    }
}
