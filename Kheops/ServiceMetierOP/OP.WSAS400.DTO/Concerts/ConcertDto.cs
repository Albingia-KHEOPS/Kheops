using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Adresses;

namespace OP.WSAS400.DTO.Concerts
{
    /// <summary>
    /// Dto du concert
    /// </summary>
    [DataContract]
    public class ConcertDto : _Concert_Base
    {
        /// <summary>
        /// Gets or sets the code inventaire.
        /// </summary>
        /// <value>
        /// The code inventaire.
        /// </value>
        [DataMember]
        public string CodeInventaire { get; set; }

        /// <summary>
        /// Gets or sets the numero ligne.
        /// </summary>
        /// <value>
        /// The numero ligne.
        /// </value>
        [DataMember]
        public string NumeroLigne { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [DataMember]
        public string Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the nom site.
        /// </summary>
        /// <value>
        /// The nom site.
        /// </value>
        [DataMember]
        public string NomSite { get; set; }

        /// <summary>
        /// Gets or sets the nature lieu.
        /// </summary>
        /// <value>
        /// The nature lieu.
        /// </value>
        [DataMember]
        public string NatureLieu { get; set; }

        /// <summary>
        /// Gets or sets the adresse.
        /// </summary>
        /// <value>
        /// The adresse.
        /// </value>
        [DataMember]
        public AdressePlatDto Adresse { get; set; }

        /// <summary>
        /// Gets or sets the date debut.
        /// </summary>
        /// <value>
        /// The date debut.
        /// </value>
        [DataMember]
        public DateTime? DateDebut { get; set; }

        /// <summary>
        /// Gets or sets the date fin.
        /// </summary>
        /// <value>
        /// The date fin.
        /// </value>
        [DataMember]
        public DateTime? DateFin { get; set; }

        /// <summary>
        /// Gets or sets the budget.
        /// </summary>
        /// <value>
        /// The budget.
        /// </value>
        [DataMember]
        public Decimal? Budget { get; set; }

        /// <summary>
        /// Gets or sets the nb event.
        /// </summary>
        /// <value>
        /// The nb event.
        /// </value>
        [DataMember]
        public int? NbEvent { get; set; }

        public ConcertDto()
        {
            this.CodeInventaire = string.Empty;
            this.Adresse = new AdressePlatDto();
            this.Budget = null;
            this.DateDebut = null ;
            this.DateFin =   null;
            this.Descriptif = string.Empty;
            this.NatureLieu = string.Empty;
            this.NbEvent = null;
            this.NomSite = string.Empty;
            this.NumeroLigne = string.Empty;
        }

    }
}
