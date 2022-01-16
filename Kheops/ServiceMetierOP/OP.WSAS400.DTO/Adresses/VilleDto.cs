using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Adresses
{
    /// <summary>
    /// Dto de la ville
    /// </summary>
    [DataContract]
  
    public class VilleDto//: _Adresse_Base
    {
        /// <summary>
        /// Gets or sets the code insee.
        /// </summary>
        /// <value>
        /// The code insee.
        /// </value>
        [DataMember]
        public string CodeInsee { get; set; }

        /// <summary>
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the code postal.
        /// </summary>
        /// <value>
        /// The code postal.
        /// </value>
        [DataMember]
        public string CodePostal { get; set; }

        /// <summary>
        /// Gets or sets the nom cedex.
        /// </summary>
        /// <value>
        /// The nom cedex.
        /// </value>
        [DataMember]
        public string NomCedex { get; set; }

        /// <summary>
        /// Gets or sets the code postal cedex.
        /// </summary>
        /// <value>
        /// The code postal cedex.
        /// </value>
        [DataMember]
        public string CodePostalCedex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [type cedex].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [type cedex]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool TypeCedex { get; set; }

        [DataMember]
        public string Departement
        {
            get
            {
                string result = String.Empty;
                if (!String.IsNullOrEmpty(this.CodePostal) && this.CodePostal.Length > 1)
                {
                    // 2 étant la longueur du département, attention à la guadeloupe...
                    result = this.CodePostal.Substring(0, 2);
                }
                else if (!String.IsNullOrEmpty(this.CodePostalCedex) && this.CodePostalCedex.Length > 1)
                {
                    result = this.CodePostalCedex.Substring(0, 2);
                }
                else
                {
                    result = this.CodePostal;
                }
                return result;
            }            
        }
        //[DataMember]
        //public PaysDto Pays { get; set; }
        ///// <summary>
        ///// Initializes a new instance of the <see cref="VilleDto"/> class.
        ///// </summary>
        //public VilleDto()
        //{
        //    this.CodeInsee = String.Empty;
        //    this.CodePostal = String.Empty;
        //    this.CodePostalCedex = String.Empty;
        //    this.Nom = String.Empty;
        //    this.NomCedex = String.Empty;
        //    this.TypeCedex = false;
        //}
    }
}