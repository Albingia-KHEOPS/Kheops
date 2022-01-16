using OP.WSAS400.DTO.Avenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.AffaireNouvelle
{
    [DataContract]
    public class AvenantInfoPageDto
    {
        [DataMember]
        public ContratDto contrat { get; set; }

        [DataMember]
        public AvenantDto avenant { get; set; }
    }
}
