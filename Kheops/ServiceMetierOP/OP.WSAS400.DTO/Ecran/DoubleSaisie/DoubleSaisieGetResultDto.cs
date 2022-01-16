using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.CabinetsCourtage;
using OP.WSAS400.DTO.Personnes;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Ecran.DoubleSaisie
{
    [DataContract]
    public class DoubleSaisieGetResultDto //: _DoubleSaisie_Base, IResult
    {

        /// <summary>
        /// Gets or sets the offre.
        /// </summary>
        /// <value>
        /// The offre.
        /// </value>
        [DataMember]
        public OffreDto Offre { get; set; }

        [DataMember]
        public List<CabinetAutreDto> CabinetAutres { get; set; }

        /// <summary>
        /// Gets or sets the souscripteurs.
        /// </summary>
        /// <value>
        /// The souscripteurs.
        /// </value>
        [DataMember]
        public List<SouscripteurDto> Souscripteurs { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus gestionnaire.
        /// </summary>
        /// <value>
        /// The motifs refus gestionnaire.
        /// </value>
        [DataMember]
        public List<ParametreDto> MotifsRefusGestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus demandeur.
        /// </summary>
        /// <value>
        /// The motifs refus demandeur.
        /// </value>
        [DataMember]
        public List<ParametreDto> MotifsRefusDemandeur { get; set; }

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
    }
}