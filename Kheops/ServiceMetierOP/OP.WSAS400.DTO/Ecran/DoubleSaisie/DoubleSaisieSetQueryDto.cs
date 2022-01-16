using System;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Ecran.DoubleSaisie
{
    /// <summary>
    /// Dto de la requete confirmationSaisieSet
    /// </summary>
    [DataContract]
    public class DoubleSaisieSetQueryDto : _DoubleSaisie_Base, IQuery
    {

        /// <summary>
        /// Gets or sets the offre double saisie.
        /// </summary>
        /// <value>
        /// The offre double saisie.
        /// </value>
        [DataMember]
        public OffreDto OffreDoubleSaisie { get; set; }

    }
}
