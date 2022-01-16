using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace OP.WSAS400.DTO.Personnes
{
    /// <summary>
    /// Result de SouscripteursGetResult
    /// </summary>
    [DataContract]
    public class SouscripteursGetResultDto //: _Personne_Base, IResult
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
        /// Gets or sets the souscripteurs dto.
        /// </summary>
        /// <value>
        /// The souscripteurs dto.
        /// </value>
        [DataMember]
        public List<SouscripteurDto> SouscripteursDto { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SouscripteursGetResultDto"/> class.
        /// </summary>
        public SouscripteursGetResultDto()
        {
            SouscripteursDto = new List<SouscripteurDto>();
        }
    }
}