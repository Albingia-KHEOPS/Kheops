using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.InformationsSpecifiquesBranche
{
    [DataContract]
    public class InformationsSpecifiquesBrancheGetQueryDto : _InformationsSpecifiquesBranche_Base, IQuery
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
        /// Gets or sets the version offre.
        /// </summary>
        /// <value>
        /// The version offre.
        /// </value>
        [DataMember]
        public int VersionOffre { get; set; }
    }
}
