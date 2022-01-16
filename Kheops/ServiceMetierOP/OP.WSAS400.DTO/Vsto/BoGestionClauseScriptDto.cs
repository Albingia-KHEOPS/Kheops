using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Vsto
{
    [DataContract]
    public class BoGestionClauseScriptDto
    {
        /// <summary>
        /// Origine
        /// </summary>
        [DataMember]
        public string Origine { get; set; }

        /// <summary>
        /// Identifiant 1
        /// </summary>
        [DataMember]
        public string Identifiant1 { get; set; }

        /// <summary>
        /// Identifiant 2
        /// </summary>
        [DataMember]
        public string Identifiant2 { get; set; }

        /// <summary>
        /// Observations
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// Script
        /// </summary>
        [DataMember]
        public string Script { get; set; }
    }
}
