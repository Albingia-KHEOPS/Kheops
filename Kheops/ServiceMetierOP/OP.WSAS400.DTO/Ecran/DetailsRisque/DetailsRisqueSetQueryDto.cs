using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Ecran.DetailsRisque
{
    [DataContract]
    public class DetailsRisqueSetQueryDto : _DetailsRisque_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the offre.
        /// </summary>
        /// <value>
        /// The offre.
        /// </value>
        [DataMember]
        public OffreDto Offre { get; set; }
    }
}
