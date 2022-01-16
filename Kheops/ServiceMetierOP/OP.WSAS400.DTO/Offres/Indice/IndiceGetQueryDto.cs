using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Indice
{
    /// <summary>
    /// Dto de la query de la méthode indiceGet
    /// </summary>
    public class IndiceGetQueryDto : _Indice_Base, IQuery 
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the date effet.
        /// </summary>
        /// <value>
        /// The date effet.
        /// </value>
        [DataMember]
        public string DateEffet { get; set; }
    }
}
