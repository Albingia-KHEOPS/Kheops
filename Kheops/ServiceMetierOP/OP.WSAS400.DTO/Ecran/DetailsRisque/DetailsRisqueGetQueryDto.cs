using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.DetailsRisque
{
    [DataContract]
    public class DetailsRisqueGetQueryDto : _DetailsRisque_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the code branche.
        /// </summary>
        /// <value>
        /// The code branche.
        /// </value>
        [DataMember]
        public string CodeBranche { get; set; }
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
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the code risque.
        /// </summary>
        /// <value>
        /// The code risque.
        /// </value>
        [DataMember]
        public int CodeRisque { get; set; }
       
    }
}
