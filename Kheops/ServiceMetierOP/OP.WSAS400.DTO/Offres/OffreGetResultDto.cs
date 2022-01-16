using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Offres
{
    /// <summary>
    /// DTO query OffreGetResult
    /// ToDo ECM Utilité ??
    /// </summary>
    [DataContract]
    public class OffreGetResultDto : _Offre_Base
    {
        /// <summary>
        /// Gets or sets the offre.
        /// </summary>
        /// <value>
        /// The offre.
        /// </value>
        [DataMember]
        public OffreDto Offre { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus.
        /// </summary>
        /// <value>
        /// The motifs refus.
        /// </value>
        [DataMember]
        public List<ParametreDto> MotifsRefus { get; set; }

    }
}


