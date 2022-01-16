using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.ConfirmationSaisie
{
    /// <summary>
    /// Classe de retour pour l'initialisation de l'écran : ConfirmationSaisie
    /// </summary>
    [DataContract]
    public class ConfirmationSaisieGetResultDto //: _ConfirmationSaisie_Base, IResult
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

        /// <summary>
        /// Gets or sets the send date.
        /// </summary>
        /// <value>
        /// The send date.
        /// </value>
        //[DataMember]
        //public DateTime SendDate { get; set; }

        /// <summary>
        /// Gets or sets the offre.
        /// </summary>
        /// <value>
        /// The offre.
        /// </value>
        [DataMember]
        public OffreDto Offre { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus.
        /// </summary>
        /// <value>
        /// The motifs refus.
        /// </value>
        [DataMember]
        public List<ParametreDto> MotifsRefus { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationSaisieGetResultDto"/> class.
        /// </summary>
        //public ConfirmationSaisieGetResultDto()
        //{
        //}
    }
}