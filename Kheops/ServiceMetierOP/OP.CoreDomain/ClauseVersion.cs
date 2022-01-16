using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    ////[Serializable]
    public class ClauseVersion
    {
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        /// <value>
        /// The numero.
        /// </value>
        public string Numero { get; set; }
        /// <summary>
        /// Gets or sets the date debut.
        /// </summary>
        /// <value>
        /// The date debut.
        /// </value>
        public DateTime DateDebut { get; set; }
        /// <summary>
        /// Gets or sets the date fin.
        /// </summary>
        /// <value>
        /// The date fin.
        /// </value>
        public DateTime DateFin { get; set; }
    }
}
