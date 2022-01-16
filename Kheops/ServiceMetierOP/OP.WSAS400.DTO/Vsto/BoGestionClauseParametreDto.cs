using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Vsto
{
    [DataContract]
    public class BoGestionClauseParametreDto
    {
        /// <summary>
        /// Code
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Libellé
        /// </summary>
        [DataMember]
        public string Libelle { get; set; }
    }
}
