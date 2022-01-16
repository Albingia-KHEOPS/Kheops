using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Ecran.Rercherchesaisie
{
    [DataContract]
    public class RechercheSaisieGetResultDto //: _RechercheSaisie_Base, IResult
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
        //public List<string> Branches { get; set; }
        public List<ParametreDto> Branches { get; set; }

        /// <summary>
        /// Gets or sets the cibles.
        /// </summary>
        /// <value>
        /// The cibles.
        /// </value>
        [DataMember]
        //public List<string> Branches { get; set; }
        public List<ParametreDto> Cibles { get; set; }

        /// <summary>
        /// Gets or sets the etats.
        /// </summary>
        /// <value>
        /// The etats.
        /// </value>
        [DataMember]
        public List<ParametreDto> Etats { get; set; }
        //public List<OP.WSAS400.DTO.enEtatOffres> Etats { get; set; }

        /// <summary>
        /// Gets or sets the situation.
        /// </summary>
        /// <value>
        /// The situation.
        /// </value>
        [DataMember]
        public List<ParametreDto> Situation { get; set; }
        //public List<OP.WSAS400.DTO.enSituationOffres> Situation { get; set; }
        [DataMember]
        public List<ParametreDto> ListRefus { get; set; }

        [DataMember]
        public List<ParametreDto> Qualites { get; set; }
    }
}
