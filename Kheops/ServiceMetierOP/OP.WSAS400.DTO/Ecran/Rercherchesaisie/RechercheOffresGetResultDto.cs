using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.Rercherchesaisie
{
    [DataContract]
    public class RechercheOffresGetResultDto //: _RechercheOffres_Base, IResult
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
        /// Gets or sets the NbCount.
        /// </summary>
        /// <value>
        /// The send NbCount.
        /// </value>
        [DataMember]
        public int NbCount { get; set; }

        /// <summary>
        /// Gets or sets the LST offres.
        /// </summary>
        /// <value>
        /// The LST offres.
        /// </value>
        [DataMember]
        public List<Offres.OffreDto> LstOffres { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RechercheOffresGetResultDto"/> class.
        /// </summary>
        public RechercheOffresGetResultDto()
        {
            this.LstOffres = new List<Offres.OffreDto>();
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RechercheOffresGetResultDto"/> class.
        ///// </summary>
        ///// <param name="argLstOffres">The arg LST offres.</param>
        //public RechercheOffresGetResultDto(List<Offres.OffreDto> argLstOffres)
        //{
        //    this.LstOffres = argLstOffres;
        //}
    }
}