using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Vsto
{
    [DataContract]
    public class BoGestionClauseSignetRubriqueDto
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Rubrique
        /// </summary>
        [DataMember]
        public string Rubrique { get; set; }

        /// <summary>
        /// Libelle
        /// </summary>
        [DataMember]
        public string Libelle { get; set; }
    }
}
