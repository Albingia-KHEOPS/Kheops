using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Branches
{
    /// <summary>
    /// Représente l'objet BranchesGetResultDto.
    /// </summary>
    [DataContract]
    public class BranchesGetResultDto : _Branches_Base, IResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [DataMember]
        public enIOAS400Results Result { get; set; }

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
        /// Obtient ou définie une liste d'objet branchesDto.
        /// </summary>
        [DataMember]
        public List<BrancheDto> BranchesDto { get; set; }

        /// <summary>
        /// Initialise une instance de la classe BranchesGetResultDto.
        /// </summary>
        public BranchesGetResultDto()
        {
            BranchesDto = new List<BrancheDto>();
        }
    }
}