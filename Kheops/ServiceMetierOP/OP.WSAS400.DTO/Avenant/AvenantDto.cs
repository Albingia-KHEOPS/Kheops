using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Avenant
{
    [DataContract]
    public class AvenantDto
    {
        [DataMember]
        public List<AvenantAlerteDto> Alertes { get; set; }
        [DataMember]
        public List<ParametreDto> TypesContrat { get; set; }
        [DataMember]
        public AvenantModificationDto AvenantModif { get; set; }
        [DataMember]
        public AvenantResiliationDto AvenantResil { get; set; }
        [DataMember]
        public AvenantRemiseEnVigueurDto AvenantRemiseEnVigueur { get; set; }
    }
}
