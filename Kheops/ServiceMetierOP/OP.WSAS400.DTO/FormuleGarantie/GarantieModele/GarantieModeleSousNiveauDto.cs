using System.Runtime.Serialization;
using System.Collections.Generic;


namespace OP.WSAS400.DTO.GarantieModele
{
    [DataContract]
    public class GarantieModeleSousNiveauDto : _GarantieModeleNiveau_Base
    {
        [DataMember]
        public List<GarantieModeleSousNiveauDto> Modeles { get; set; }

        [DataMember]
        public bool IsLastAuthorizedNiveau { get; private set; }

        public GarantieModeleSousNiveauDto()
        {
            IsLastAuthorizedNiveau = true;
            Modeles = null;
        }

        public GarantieModeleSousNiveauDto(bool isLastNieau)
        {
            IsLastAuthorizedNiveau = isLastNieau;
            Modeles = new List<GarantieModeleSousNiveauDto>();
        }

    }
}
