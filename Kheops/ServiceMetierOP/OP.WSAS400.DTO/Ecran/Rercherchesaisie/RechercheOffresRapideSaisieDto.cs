using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.Rercherchesaisie
{
    [DataContract]
    public class RechercheOffresRapideSaisieDto
    {
        [DataMember]
        public IList<ParametreDto> TypeTraitements { get; set; }
        [DataMember]
        public IList<ParametreDto> Branches { get; set; }
        [DataMember]
        public IList<ParametreDto> Cibles { get; set; }
        [DataMember]
        public IList<ParametreDto> Periodicites { get; set; }
    }
}
