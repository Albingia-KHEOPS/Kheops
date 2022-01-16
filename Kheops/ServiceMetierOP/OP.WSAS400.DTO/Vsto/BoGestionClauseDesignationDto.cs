using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Vsto
{
    public class BoGestionClauseDesignationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Désignation de la clause
        /// </summary>
        [DataMember]
        public string Designation { get; set; }
    }
}
