using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Ecran.ModifierOffre
{
    /// <summary>
    /// Dto de la requete SaisieOffreSet
    /// </summary>
    [DataContract]
    public class ModifierOffreSetQueryDto : _ModifierOffre_Base, IQuery
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
