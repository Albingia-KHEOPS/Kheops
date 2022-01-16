
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Partenaire
{
    [DataContract]
    public class PartenairesBaseDetailsDto
    {

        #region CourtierGestionnaire
        [DataMember]
        [Column(Name = "CODECOURTIERGESTIONNAIRE")]
        public string CodeCourtierGestionnaire { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERGESTIONNAIRE")]
        public string NomCourtierGestionnaire { get; set; }
        #endregion

        #region interlocuteur
        [DataMember]
        [Column(Name = "CODEINTERLOCUTEUR")]
        public int? CodeInterlocuteur { get; set; }
        [DataMember]
        [Column(Name = "NOMINTERLOCUTEUR")]
        public string NomInterlocuteur { get; set; }
        #endregion

        #region CourtierApporteur
        [DataMember]
        [Column(Name = "CODECOURTIERAPPORTEUR")]
        public string CodeCourtierApporteur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERAPPORTEUR")]
        public string NomCourtierApporteur { get; set; }
        #endregion

        #region CourtierPayeur
        [DataMember]
        [Column(Name = "CODECOURTIERPAYEUR")]
        public string CodeCourtierPayeur { get; set; }
        [DataMember]
        [Column(Name = "NOMCOURTIERPAYEUR")]
        public string NomCourtierPayeur { get; set; }
        #endregion

        #region PreneurAssurance
        [DataMember]
        [Column(Name = "CODEPRENEURASSURANCE")]
        public string CodePreneurAssurance { get; set; }
        [DataMember]
        [Column(Name = "NOMPRENEURASSURANCE")]
        public string NomPreneurAssurance { get; set; }
        #endregion

    }
}