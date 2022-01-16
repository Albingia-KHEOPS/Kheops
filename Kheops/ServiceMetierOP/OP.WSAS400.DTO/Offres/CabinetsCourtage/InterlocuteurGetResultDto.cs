using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    /// <summary>
    /// Dto du result de la méthode interlocuteurGet
    /// </summary>
    [DataContract]
    public class InterlocuteurGetResultDto //: _Interlocuteur_Base, IResult
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
        /// Gets or sets the interlocuteurs.
        /// </summary>
        /// <value>
        /// The interlocuteurs.
        /// </value>
        [DataMember]
        public List<InterlocuteurDto> Interlocuteurs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterlocuteurGetResultDto"/> class.
        /// </summary>
        //public InterlocuteurGetResultDto()
        //{
        //   // Message = _DTO_Base._undefinedString;
        //}
    }
}
