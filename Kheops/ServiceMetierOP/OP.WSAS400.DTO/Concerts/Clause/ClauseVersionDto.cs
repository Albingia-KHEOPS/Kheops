using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Clause
{
    [DataContract]
    public class ClauseVersionDto
    {
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        /// <value>
        /// The numero.
        /// </value>
        [DataMember]
        public string Numero { get; set; }
        /// <summary>
        /// Gets or sets the date debut.
        /// </summary>
        /// <value>
        /// The date debut.
        /// </value>
        [DataMember]
        public DateTime DateDebut { get; set; }
        /// <summary>
        /// Gets or sets the date fin.
        /// </summary>
        /// <value>
        /// The date fin.
        /// </value>
        [DataMember]
        public DateTime DateFin { get; set; }
    }
}
