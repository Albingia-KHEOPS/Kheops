using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.Indice
{
    [DataContract]
    public class IndiceDto : _Indice_Base
    {
        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        [DataMember]
        public string Valeur { get; set; }

        public IndiceDto()
        {
            this.Valeur = string.Empty;
        }
    }
}
