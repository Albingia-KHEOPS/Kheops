using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.GarantieModele
{
    [DataContract]
    public class GarantieModeleNiveau1Dto : _GarantieModeleNiveau_Base
    {
        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string AlimAssiette { get; set; }

        [DataMember]
        public List<GarantieModeleSousNiveauDto> Modeles { get; set; }

    }
}
