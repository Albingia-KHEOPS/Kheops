using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Concerts;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Offres.Aperiteur
{
    [DataContract]
    public class AperiteurDto //: _Aperiteur_Base
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        [Column(Name = "CODE")]
        public string Code { get; set; }

        [DataMember]
        [Column(Name = "CODENUM")]
        public Int64 CodeNum { get; set; }

        /// <summary>
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        [Column(Name = "NOM")]
        public string Nom { get; set; }

        //public AperiteurDto()
        //{
        //    this.Code = string.Empty;
        //    this.Nom = string.Empty;
        //}
    }
}
