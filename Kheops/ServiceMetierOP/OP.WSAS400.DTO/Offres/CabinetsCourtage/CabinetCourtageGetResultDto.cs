using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    /// <summary>
    /// Dto du resultat de la méhode CabinetCourtageGet
    /// </summary>
    [DataContract]
    public class CabinetCourtageGetResultDto //: _CabinetsCourtage_Base, IResult
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
        /// Gets or sets the cabinet courtage.
        /// </summary>
        /// <value>
        /// The cabinet courtage.
        /// </value>
        [DataMember]
        public List<CabinetCourtageDto> CabinetCourtages { get; set; }

        /// <summary>
        /// Get the cabinet courtage count
        /// </summary>
        [DataMember]
        public int CabinetCourtageCount { get; set; }
        /// <summary>
        /// Gets or sets the souscripteurs.
        /// LJO ! Je mets une string pour des raisons d'optim mais normalement je préfère une classe
        /// </summary>
        /// <value>
        /// The souscripteurs.
        /// </value>
        [DataMember]
        public List<Parametres.ParametreDto> Souscripteurs { get; set; }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="CabinetCourtageGetResultDto"/> class.
        ///// </summary>
        //public CabinetCourtageGetResultDto()
        //{
        //    Message = _DTO_Base._undefinedString;
        //}
    }
}
