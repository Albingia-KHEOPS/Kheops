using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Adresses
{
    /// <summary>
    /// Dto du pays
    /// </summary>
    [DataContract]
    
    public class PaysDto //: _Adresse_Base
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

        ///// <summary>
        ///// Initializes a new instance of the <see cref="PaysDto"/> class.
        ///// </summary>
        //public PaysDto()
        //{
        //    this.Code = String.Empty;
        //    this.Nom = String.Empty;
        //}
    }
}