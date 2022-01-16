using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Assures;

namespace OP.WSAS400.DTO.Adresses
{
    /// <summary>
    /// Dto de l'adresse
    /// </summary>
    [DataContract]
   
    public class AdresseDto // : _Adresse_Base
    {
        /// <summary>
        /// Gets or sets the numero chrono.
        /// </summary>
        /// <value>
        /// The numero chrono.
        /// </value>
        [DataMember]
        public int? NumeroChrono { get; set; }

        /// <summary>
        /// Gets or sets the matricule hexavia.
        /// </summary>
        /// <value>
        /// The matricule hexavia.
        /// </value>
        [DataMember]
        public string MatriculeHexavia { get; set; }
        
        /// <summary>
        /// Gets or sets the batiment.
        /// </summary>
        /// <value>
        /// The batiment.
        /// </value>
        [DataMember]
        public string Batiment { get; set; }
        
        /// <summary>
        /// Gets or sets the numero voie.
        /// </summary>
        /// <value>
        /// The numero voie.
        /// </value>
        [DataMember]
        public string NumeroVoie { get; set; }
       
        /// <summary>
        /// Gets or sets the extension voie.
        /// </summary>
        /// <value>
        /// The extension voie.
        /// </value>
        [DataMember]
        public string ExtensionVoie { get; set; }
       
        /// <summary>
        /// Gets or sets the nom voie.
        /// </summary>
        /// <value>
        /// The nom voie.
        /// </value>
        [DataMember]
        public string NomVoie { get; set; }
      
        /// <summary>
        /// Gets or sets the boite postale.
        /// </summary>
        /// <value>
        /// The boite postale.
        /// </value>
        [DataMember]
        public string BoitePostale { get; set; }
     
        /// <summary>
        /// Gets or sets the ville.
        /// </summary>
        /// <value>
        /// The ville.
        /// </value>
        [DataMember]
        public VilleDto Ville { get; set; }
       
        /// <summary>
        /// Gets or sets the pays dto.
        /// </summary>
        /// <value>
        /// The pays dto.
        /// </value>
        [DataMember]
        public PaysDto Pays { get; set; }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AdresseDto"/> class.
        ///// </summary>
        //public AdresseDto()
        //{
        //    this.Batiment = String.Empty;
        //    this.BoitePostale = String.Empty;
        //    this.ExtensionVoie = String.Empty;
        //    this.MatriculeHexavia = String.Empty;
        //    this.NomVoie = String.Empty;
        //    this.NumeroChrono = int.MinValue;
        //    this.NumeroVoie = String.Empty;
        //    this.Ville = new VilleDto();
        //    this.Pays = new PaysDto();
        //}
    }
}