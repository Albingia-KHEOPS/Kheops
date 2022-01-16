using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.WSAS400.DTO.Offres;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.CreationSaisie
{
    public class CreationSaisieSetQueryDto : _CreationSaisie_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the offre dto.
        /// </summary>
        /// <value>
        /// The offre dto.
        /// </value>
        [DataMember]
        public OffreDto Offre { get; set; }
    }
}
