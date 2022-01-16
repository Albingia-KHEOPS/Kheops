using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Personnes;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.CreationSaisie
{
    [DataContract]
    public class CreationSaisieGetResultDto //: _CreationSaisie_Base, IResult
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
        /// Gets or sets the branches.
        /// </summary>
        /// <value>
        /// The branches.
        /// </value>
        [DataMember]
        public List<BrancheDto> Branches { get; set; }

        [DataMember]
        public List<CibleDto> Cibles { get; set; }



        /// <summary>
        /// Gets or sets the mots cles.
        /// </summary>
        /// <value>
        /// The mots cles.
        /// </value>
        [DataMember]
        public List<ParametreDto> MotsCles { get; set; }
    }
}
