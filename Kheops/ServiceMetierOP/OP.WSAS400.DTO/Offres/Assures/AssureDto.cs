using System.Collections.Generic;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Adresses;
using System;

namespace OP.WSAS400.DTO.Offres.Assures
{
    /// <summary>
    /// DTO de l'assure
    /// </summary>
    [DataContract]
    
    public class AssureDto //: _Assure_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the nom assure.
        /// </summary>
        /// <value>
        /// The nom assure.
        /// </value>
        [DataMember]
        public string NomAssure { get; set; }

        /// <summary>
        /// Gets or sets the nom secondaires.
        /// </summary>
        /// <value>
        /// The nom secondaires.
        /// </value>
        [DataMember]
        public List<string> NomSecondaires { get; set; }

        ///// <summary>
        ///// Gets or sets the adresse.
        ///// </summary>
        ///// <value>
        ///// The adresse.
        ///// </value>
        [DataMember]
        public AdressePlatDto Adresse { get; set; }

        /// <summary>
        /// Gets or sets the siren.
        /// </summary>
        /// <value>
        /// The siren.
        /// </value>
        [DataMember]
        public int Siren { get; set; }

        [DataMember]
        public string TelephoneBureau { get; set; }

        /// <summary>
        /// Gets or sets if PreneurEstAssure.
        /// </summary>
        [DataMember]
        public bool PreneurEstAssure { get; set; }

        [DataMember]
        public int NbSinistres { get; set; }

        [DataMember]
        public bool RetardsPaiements { get; set; }

        [DataMember]
        public bool Impayes { get; set; }

        [DataMember]
        public bool EstActif { get; set; }
    }
}