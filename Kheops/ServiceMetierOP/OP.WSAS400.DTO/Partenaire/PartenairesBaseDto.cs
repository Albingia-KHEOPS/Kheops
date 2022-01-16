using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Partenaire
{
    [DataContract]
    public class PartenairesBaseDto
    {

        #region CourtierGestionnaire
        [DataMember]
        public PartenaireDto CourtierGestionnaire { get; set; }
        #endregion

        #region CourtierApporteur     
        [DataMember]
        public PartenaireDto CourtierApporteur { get; set; }
        #endregion

        #region CourtierPayeur
        [DataMember]
        public PartenaireDto CourtierPayeur { get; set; }
        #endregion

        #region PreneurAssurance
        [DataMember]
        public PartenaireDto PreneurAssurance { get; set; }
        #endregion

    }
}