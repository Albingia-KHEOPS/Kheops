using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Risque;

namespace OP.WSAS400.DTO.Ecran.DetailsObjetRisque
{
    [DataContract]
    public class DetailsObjetRisqueSetResultDto //: _DetailsObjetRisque_Base, IResult
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
        ///// Gets the receive date.
        ///// </summary>
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
        /// Gets or sets the code risque.
        /// </summary>
        /// <value>
        /// The code risque.
        /// </value>
        [DataMember]
        public string CodeRisque { get; set; }

        /// <summary>
        /// Gets or sets the risque.
        /// </summary>
        /// <value>
        /// The risque.
        /// </value>
        [DataMember]
        public RisqueDto Risque { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsRisqueSetResultDto"/> class.
        /// </summary>
        public DetailsObjetRisqueSetResultDto()
        {

        }
    }
}
