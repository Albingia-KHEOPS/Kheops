using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.DetailsInventaire
{
    [DataContract]
    public class DetailsInventaireDelQueryDto : _DetailsInventaire_Base, IQuery
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
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DataMember]
        public int? Version { get; set; }
        /// <summary>
        /// Gets or sets the code risque.
        /// </summary>
        /// <value>
        /// The code risque.
        /// </value>
        [DataMember]
        public int CodeRisque { get; set; }
        /// <summary>
        /// Gets or sets the code objet.
        /// </summary>
        /// <value>
        /// The code objet.
        /// </value>
        [DataMember]
        public int CodeObjet { get; set; }
        /// <summary>
        /// Gets or sets the code inventaire.
        /// </summary>
        /// <value>
        /// The code inventaire.
        /// </value>
        [DataMember]
        public string CodeInventaire { get; set; }
        /// <summary>
        /// Gets or sets the num description.
        /// </summary>
        /// <value>
        /// The num description.
        /// </value>
        [DataMember]
        public string NumDescription { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}
