using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Risque.Inventaire;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.DetailsInventaire
{
    [DataContract]
    public class DetailsInventaireGetResultDto //: _DetailsInventaire_Base, IResult
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
        /// Gets or sets the inventaire.
        /// </summary>
        /// <value>
        /// The inventaire.
        /// </value>
        [DataMember]
        public InventaireDto Inventaire { get; set; }

        /// <summary>
        /// Gets or sets the unites.
        /// </summary>
        /// <value>
        /// The unites.
        /// </value>
        [DataMember]
        public List<ParametreDto> Unites { get; set; }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        [DataMember]
        public List<ParametreDto> Types { get; set; }
    }
}
