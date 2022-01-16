using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Offres.CabinetsCourtage
{
    /// <summary>
    /// Dto de la query de la méthode InterlocuteurGet
    /// </summary>
    [DataContract]
    public class InterlocuteurGetQueryDto : _Interlocuteur_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the nom.
        /// </summary>
        /// <value>
        /// The nom.
        /// </value>
        [DataMember]
        public string Nom { get; set; }

        /// <summary>
        /// Gets or sets the code cabinet courtage.
        /// </summary>
        /// <value>
        /// The code cabinet courtage.
        /// </value>
        [DataMember]
        public int CodeCabinetCourtage { get; set; }
    }
}
