using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Ecran.InformationsSpecifiquesBranche
{
    [DataContract]
    public class InformationsSpecifiquesBrancheSetQueryDto : _InformationsSpecifiquesBranche_Base, IQuery
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
