using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Branches
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class BonificationsDto
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        [Column(Name="TYPE")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BonificationsDto"/> is bonification.
        /// </summary>
        /// <value>
        ///   <c>true</c> if bonification; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Bonification { get; set; }
        [Column(Name = "ISBONIFICATION")]
        public string IsBonification { get; set; }

        /// <summary>
        /// Gets or sets the taux bonification.
        /// </summary>
        /// <value>
        /// The taux bonification.
        /// </value>
        [DataMember]
        public string TauxBonification { get; set; }
        [Column(Name = "TAUX")]
        public Single Taux { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BonificationsDto"/> is anticipe.
        /// </summary>
        /// <value>
        ///   <c>true</c> if anticipe; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Anticipe { get; set; }
        [Column(Name = "ISANTICIPE")]
        public string IsAnticipe { get; set; }
    }
}
