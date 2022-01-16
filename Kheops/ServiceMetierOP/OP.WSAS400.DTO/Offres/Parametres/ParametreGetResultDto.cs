using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Parametres
{
    [DataContract]
    public class ParametreGetResultDto : _Parametre_Base 
    {
        /// <summary>
        /// Gets or sets the parametre dto.
        /// </summary>
        /// <value>
        /// The parametre dto.
        /// </value>
        [DataMember ]
        public List<ParametreDto> ParametreDto { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametreGetResultDto"/> class.
        /// </summary>
        public ParametreGetResultDto()
        {
            ParametreDto = new List<ParametreDto>();
        }
    }
}