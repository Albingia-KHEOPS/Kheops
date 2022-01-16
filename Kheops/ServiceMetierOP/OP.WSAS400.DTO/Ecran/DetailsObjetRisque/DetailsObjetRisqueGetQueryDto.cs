using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.DetailsObjetRisque
{
    [DataContract]
    public class DetailsObjetRisqueGetQueryDto : _DetailsObjetRisque_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the code branche.
        /// </summary>
        /// <value>
        /// The code branche.
        /// </value>
        [DataMember]
        public string CodeBranche { get; set; }
        [DataMember]
        public string CodeOffre { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string CodeRisque { get; set; }
        [DataMember]
        public string CodeObjet { get; set; }
    }
}
