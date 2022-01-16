using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres
{
    /// <summary>
    /// Dto Query OffreGet
    /// </summary>
    [DataContract]
    public class OffreGetQueryDto : _Offre_Base
    {
        /// <summary>
        /// Gets or sets the code offre.
        /// </summary>
        /// <value>
        /// The code offre.
        /// </value>
        [DataMember]
        public string CodeOffre { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OffreGetQueryDto"/> class.
        /// Ne pas utiliser
        /// </summary>
        public OffreGetQueryDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OffreGetQueryDto"/> class.
        /// </summary>
        /// <param name="codeOffre">The code offre.</param>
        public OffreGetQueryDto(string codeOffre)
        {
            this.CodeOffre = codeOffre;
        }
    }
}