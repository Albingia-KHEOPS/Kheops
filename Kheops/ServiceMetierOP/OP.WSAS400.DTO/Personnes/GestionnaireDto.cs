using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Branches;
using System.Data.Linq.Mapping;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;

namespace OP.WSAS400.DTO.Personnes
{
    /// <summary>
    /// Dto du gestionnaire
    /// </summary>
    [DataContract]
    public class GestionnaireDto //: _Personne_Base
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DataMember]
        [Column(Name = "PBGES")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        [DataMember]
        [Column(Name = "UTPFX")]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        [Column(Name = "UTNOM")]
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the prenom.
        /// </summary>
        /// <value>
        /// The prenom.
        /// </value>
        [DataMember]
        [Column(Name = "UTPNM")]
        public string Prenom { get; set; }

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        [DataMember]
        public BrancheDto Branche { get; set; }

        [Column(Name = "UTBRA")]
        public string SBranche { get; set; }

        [Column(Name = "BUDBU")]
        [DataMember]
        public string NomDelegation { get; set; }

        [Column(Name = "UTGES")]
        [DataMember]
        public string IsGestionnaire { get; set; }

        public GestionnaireDto()
        {
            this.Branche = null;
            this.Id = String.Empty;
            this.Login = String.Empty ;
            this.Nom = String.Empty;
            this.Prenom = String.Empty;          
        }
    }
}