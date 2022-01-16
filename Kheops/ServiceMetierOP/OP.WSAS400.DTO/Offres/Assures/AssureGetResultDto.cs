using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Assures
{
    [DataContract]
   

    public class AssureGetResultDto //: _Assure_Base, IResult
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

        /// <summary>
        /// Gets or sets the receive date.
        /// </summary>
        /// <value>
        /// The receive date.
        /// </value>
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
        /// Gets or sets the assures.
        /// </summary>
        /// <value>
        /// The assures.
        /// </value>
        [DataMember]
        public List<AssureDto> Assures { get; set; }
        /// <summary>
        /// Get the assures count
        /// </summary>
        [DataMember]
        public int AssuresCount { get; set; }
    }

   
}
