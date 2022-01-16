using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace OP.WSAS400.DTO.Offres.Branches
{
    /// <summary>
    /// DTO de la branche
    /// </summary>
    [DataContract]
    public class BrancheDto //: _Offre_Base
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
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the cible.
        /// </summary>
        /// <value>
        /// The cible.
        /// </value>
        [DataMember]
        public CibleDto Cible { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrancheDto"/> class.
        /// </summary>
        //public BrancheDto()
        //{
        //    this.Code = String.Empty;
        //    this.Nom = String.Empty;
        //}
    }
}