
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Partenaire
{
    [DataContract]
    public class PartenairesDto : PartenairesBaseDto
    {

        public PartenairesDto() : base()
        {

        }
        public PartenairesDto(PartenairesBaseDto p)
        {
            CourtierGestionnaire = p.CourtierGestionnaire;
            CourtierApporteur = p.CourtierApporteur;
            CourtierPayeur = p.CourtierPayeur;
            PreneurAssurance = p.PreneurAssurance;
        }
        #region AssuresAdditionnels
        [DataMember]
        public List<PartenaireDto> AssuresAdditionnels { get; set; }
        #endregion

        #region CourtiersAdditionnels
        [DataMember]
        public List<PartenaireDto> CourtiersAdditionnels { get; set; }
        #endregion
        
        #region Coassureurs
        [DataMember]
        public List<PartenaireDto> Coassureurs { get; set; }
        #endregion

        #region Intervenants
        [DataMember]
        public List<PartenaireDto> Intervenants { get; set; }
        #endregion

    }
}