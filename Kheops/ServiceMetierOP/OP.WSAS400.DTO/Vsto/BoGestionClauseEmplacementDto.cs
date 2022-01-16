using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Vsto
{
    public class BoGestionClauseEmplacementDto
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Origine
        /// </summary>
        [DataMember]
        public string Origine { get; set; }

        /// <summary>
        /// Emplacement du fichier
        /// </summary>
        [DataMember]
        public string Emplacement { get; set; }
    }
}
