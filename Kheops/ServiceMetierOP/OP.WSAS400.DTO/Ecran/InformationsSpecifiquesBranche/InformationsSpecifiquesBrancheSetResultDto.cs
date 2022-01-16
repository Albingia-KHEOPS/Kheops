using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.InformationsSpecifiquesBranche
{
    [DataContract]
    public class InformationsSpecifiquesBrancheSetResultDto //: _InformationsSpecifiquesBranche_Base, IResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        //[DataMember]
        //public enIOAS400Results Result { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the receive date.
        /// </summary>
        /// <value>
        /// The receive date.
        /// </value>
        [DataMember]
        public DateTime ReceiveDate { get; set; }

        /// <summary>
        /// Gets or sets the send date.
        /// </summary>
        /// <value>
        /// The send date.
        /// </value>
        [DataMember]
        public DateTime SendDate { get; set; }

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
        public string Version { get; set; }
    }
}
