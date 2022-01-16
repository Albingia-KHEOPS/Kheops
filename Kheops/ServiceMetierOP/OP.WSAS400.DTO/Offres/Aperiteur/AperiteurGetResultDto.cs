using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Aperiteur
{
    [DataContract]
    public class AperiteurGetResultDto// : _Aperiteur_Base, IResult
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
        //[DataMember]
        //public string Message { get; set; }

        ///// <summary>
        ///// Gets or sets the receive date.
        ///// </summary>
        ///// <value>
        ///// The receive date.
        ///// </value>
        //[DataMember]
        //public DateTime ReceiveDate { get; set; }

        ///// <summary>
        ///// Gets or sets the send date.
        ///// </summary>
        ///// <value>
        ///// The send date.
        ///// </value>
        //[DataMember]
        //public DateTime SendDate { get; set; }

        /// <summary>
        /// Gets or sets the aperiteurs dto.
        /// </summary>
        /// <value>
        /// The aperiteurs dto.
        /// </value>
        [DataMember]
        public List<AperiteurDto> AperiteursDto { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AperiteurGetResultDto"/> class.
        /// </summary>
        public AperiteurGetResultDto()
        {
            AperiteursDto = new List<AperiteurDto>();
        }
    }
}
