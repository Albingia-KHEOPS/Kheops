using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres.Branches;
using OP.WSAS400.DTO.Offres.Parametres;

namespace OP.WSAS400.DTO.Clause
{
    [DataContract]
    public class ClausierGetResultDto : _Clause_Base
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
        /// Gets the receive date.
        /// </summary>
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
        /// Gets or sets the branches.
        /// </summary>
        /// <value>
        /// The branches.
        /// </value>
        [DataMember]
        public List<BrancheDto> Branches { get; set; }

        /// <summary>
        /// Gets or sets the mots cles.
        /// </summary>
        /// <value>
        /// The mots cles.
        /// </value>
        [DataMember]
        public List<ParametreDto> MotsCles { get; set; }

        /// <summary>
        /// Gets or sets the clauses.
        /// </summary>
        /// <value>
        /// The clauses.
        /// </value>
        [DataMember]
        public List<ClauseDto> Clauses { get; set; }
    }
}
