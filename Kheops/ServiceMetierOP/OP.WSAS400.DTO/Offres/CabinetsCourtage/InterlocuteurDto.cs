using System;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    /// <summary>
    /// DTO de l'interlocuteur
    /// </summary>
    [DataContract]
    public class InterlocuteurDto : _CabinetsCourtage_Base
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        [Column(Name = "PBIN5")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the id secondaire.
        /// </summary>
        /// <value>
        /// The id secondaire.
        /// </value>
        [DataMember]
        public string IdSecondaire { get; set; }

        /// <summary>
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        [Column(Name = "NOM")]
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [est valide].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [est valide]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool EstValide { get; set; }

        [DataMember]
        [Column(Name = "TCBUR")]
        public string CodeDelegation { get; set; }
        [DataMember]
        [Column(Name = "BUDBU")]
        public string NomDelegation { get; set; }

        [Column(Name = "TCGEP")]
        public string SEstValide { get; set; }

        #region Cabinet courtage
        [Column(Name = "TNICT")]
        public int CabinetCourtageCode { get; set; }

        [Column(Name = "TNNOMCAB")]
        public string CabinetCourtageNom { get; set; }

        [Column(Name = "TCTYP")]
        public string CabinetCourtageType { get; set; }

        [Column(Name = "TCGEPC")]
        public string CabinetCourtageSEstValide { get; set; }

        [Column(Name = "TCCP")]
        public string CabinetCourtageCP { get; set; }

        [Column(Name = "TCVILC")]
        public string CabinetCourtageVille { get; set; }

        [Column(Name = "TCFVA")]
        public Int32 FinValiditeAnnee { get; set; }

        [Column(Name = "TCFVM")]
        public Int32 FinValiditeMois { get; set; }

        [Column(Name = "TCFVJ")]
        public Int32 FinValiditeJour { get; set; }
        [Column(Name = "TCFVAC")]
        public Int32 FinValiditeAnneeInterlocteur { get; set; }

        [Column(Name = "TCFVMC")]
        public Int32 FinValiditeMoisInterlocteur { get; set; }

        [Column(Name = "TCFVJC")]
        public Int32 FinValiditeJourInterlocteur { get; set; }

        [DataMember]
        public DateTime? FinValidite { get; set; }

        [DataMember]
        public bool DemarcheCom { get; set; }

        /// <summary>
        /// Gets or sets the cabinet courtage.
        /// </summary>
        /// <value>
        /// The cabinet courtage.
        /// </value>
        [DataMember]
        public CabinetCourtageDto CabinetCourtage { get; set; }

        #endregion
    }
}