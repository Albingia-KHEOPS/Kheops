using OP.WSAS400.DTO.Common;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;


namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public class RegularisationBandeauContratDto
    {
        [DataMember]
        public IdContratDto BasicInfo { get; set; }

        [DataMember]
        [Column(Name = "IDENTIFICATION")]
        public string Identification { get; set; }

        [DataMember]
        [Column(Name = "ASSURE")]
        public string Assure { get; set; }

        [DataMember]
        [Column(Name = "SOUSCRIPTEUR")]
        public string Souscripteur { get; set; }

        [DataMember]
        [Column(Name = "DEBUT_EFFET")]
        public string DateDebutEffet { get; set; }

        [DataMember]
        [Column(Name = "FIN_EFFET")]
        public string DateFinEffet { get; set; }

        [DataMember]
        [Column(Name = "PERIODICITE")]
        public string Periodicite { get; set; }

        [DataMember]
        [Column(Name = "ECHEANCE")]
        public string Echeance { get; set; }

        [DataMember]
        [Column(Name = "NATURE")]
        public string NatureContrat { get; set; }

        [DataMember]
        [Column(Name = "COURTIER")]
        public string Courtier { get; set; }

        [DataMember]
        [Column(Name = "RETOUR_P")]
        public string RetourPiece { get; set; }

        [DataMember]
        [Column(Name = "OBSERVATION")]
        public string Observation { get; set; }

        [DataMember]
        [Column(Name = "GESTIONNAIRE")]
        public string Gestionnaire { get; set; }

        [DataMember]
        [Column(Name = "CONTRAT_MERE")]
        public string ContratMere { get; set; }

        [DataMember]
        [Column(Name = "TYPE_CONTRAT")]
        public string TypeContrat { get; set; }
    }
}
