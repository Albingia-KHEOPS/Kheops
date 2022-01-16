using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Personnes
{
    /// <summary>
    /// Result de GestionnairesGetResult
    /// </summary>
    [DataContract]
    public class GestionnairesGetResultDto //: _Personne_Base, IResult
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
        /// Gets or sets the gestionnaires dto.
        /// </summary>
        /// <value>
        /// The gestionnaires dto.
        /// </value>
        [DataMember]
        public List<GestionnaireDto> GestionnairesDto { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GestionnairesGetResultDto"/> class.
        /// </summary>
        public GestionnairesGetResultDto()
        {
            GestionnairesDto = new List<GestionnaireDto>();
        }
    }
}