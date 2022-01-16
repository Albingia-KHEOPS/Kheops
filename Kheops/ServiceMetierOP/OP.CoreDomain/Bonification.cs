using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class Bonifications
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Bonification"/> is bonification.
        /// </summary>
        /// <value>
        ///   <c>true</c> if bonification; otherwise, <c>false</c>.
        /// </value>
        public bool Bonification { get; set; }

        /// <summary>
        /// Gets or sets the taux bonification.
        /// </summary>
        /// <value>
        /// The taux bonification.
        /// </value>
        public string TauxBonification { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Bonification"/> is anticipe.
        /// </summary>
        /// <value>
        ///   <c>true</c> if anticipe; otherwise, <c>false</c>.
        /// </value>
        public bool Anticipe { get; set; }
    }
}
