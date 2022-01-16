using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Volet
{
    [DataContract]
    public class VoletGetResultDto : _DTO_Base, IResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [DataMember]
        public enIOAS400Results Result { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets the receive date.
        /// </summary>
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
        /// Gets or sets the volets.
        /// </summary>
        /// <value>
        /// The volets.
        /// </value>
        [DataMember]
        public List<VoletDto> Volets { get; set; }

        public VoletGetResultDto()
        {
            this.Volets = new List<VoletDto>();
        }

    }
}
