using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Adresses;


namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    /// <summary>
    /// DTO du cabinet de courtage
    /// </summary>
    [DataContract]
    public class CabinetCourtageDto //: _CabinetsCourtage_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the nom cabinet.
        /// </summary>
        /// <value>
        /// The nom cabinet.
        /// </value>
        [DataMember]
        public string NomCabinet { get; set; }

        /// <summary>
        /// Gets or sets the nom secondaires.
        /// </summary>
        /// <value>
        /// The nom secondaires.
        /// </value>
        [DataMember]
        public List<string> NomSecondaires { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the adresse.
        /// </summary>
        /// <value>
        /// The adresse.
        /// </value>
        [DataMember]
        public AdressePlatDto Adresse { get; set; }

        /// <summary>
        /// Gets or sets the interlocuteurs.
        /// </summary>
        /// <value>
        /// The interlocuteurs.
        /// </value>
        [DataMember]
        public List<InterlocuteurDto> Interlocuteurs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether at least one Interlocuteur is valid
        /// </summary>
        [DataMember]
        public bool ValideInterlocuteur { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [est valide].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [est valide]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool EstValide { get; set; }

        /// <summary>
        /// Gets or sets the fin validite.
        /// </summary>
        /// <value>
        /// The fin validite.
        /// </value>
        [DataMember]
        public DateTime? FinValidite { get; set; }

        /// <summary>
        /// Gets or sets the delegation.
        /// </summary>
        /// <value>
        /// The delegation.
        /// </value>
        [DataMember]
        public DelegationDto Delegation { get; set; }

        [DataMember]
        public string TelephoneBureau { get; set; }

        [DataMember]
        public string EmailBureau { get; set; }

        [DataMember]
        public string CodeEncaissement { get; set; }

        [DataMember]
        public string LibEncaissement { get; set; }

        #region Interlocuteur principal
        [DataMember]
        public string NomInterlocuteur { get; set; }

        [DataMember]
        public string FonctionInterlocuteur { get; set; }

        [DataMember]
        public string TelephoneInterlocuteur { get; set; }

        [DataMember]
        public string EmailInterlocuteur { get; set; }

        #endregion

        [DataMember]
        public string Inspecteur { get; set; }

        [DataMember]
        public bool DemarcheCom { get; set; }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetCourtageDto"/> class.
        /// </summary>
        public CabinetCourtageDto()
        {
            this.Adresse = null;
            this.Code = 0;
            this.Delegation = null;
            this.EstValide = false;
            this.FinValidite = null;
            this.Interlocuteurs = null;
            this.NomCabinet = null;
            this.NomSecondaires = new List<string>();
            this.Type = String.Empty;
        }
    }
}