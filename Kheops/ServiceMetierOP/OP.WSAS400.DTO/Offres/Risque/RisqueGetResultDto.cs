using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Risque
{
    [DataContract]
    public class RisqueGetResultDto : _Risque_Base, IResult
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
        /// Gets or sets the risque dto.
        /// </summary>
        /// <value>
        /// The risque dto.
        /// </value>
        [DataMember]
        public RisqueDto RisqueDto { get; set; }

        [DataMember]
        public List<RisqueDto> RisquesDto { get; set; }

        public RisqueGetResultDto()
        {
            RisqueDto = new RisqueDto();
            RisquesDto = new List<RisqueDto>();
        }
    }
}
