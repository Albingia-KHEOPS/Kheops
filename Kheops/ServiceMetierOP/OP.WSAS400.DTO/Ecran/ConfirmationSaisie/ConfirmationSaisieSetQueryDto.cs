using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.ConfirmationSaisie
{
    /// <summary>
    /// Dto de la requete confirmationSaisieSet
    /// </summary>
    [DataContract]
    public class ConfirmationSaisieSetQueryDto : _ConfirmationSaisie_Base, IQuery
    {
        /// <summary>
        /// Gets or sets a value indicating whether [refus immediat].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [refus immediat]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RefusImmediat { get; set; }

        [DataMember]
        public bool Attente { get; set; }

        /// <summary>
        /// Gets or sets the motif refus.
        /// </summary>
        /// <value>
        /// The motif refus.
        /// </value>
        [DataMember]
        public ParametreDto MotifRefus { get; set; }

        /// <summary>
        /// Gets or sets the code offre.
        /// </summary>
        /// <value>
        /// The code offre.
        /// </value>
        [DataMember]
        public string CodeOffre { get; set; }
    }
}
