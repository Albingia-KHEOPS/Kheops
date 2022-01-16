using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;

namespace OP.WSAS400.DTO.Ecran.DetailsObjetRisque
{
    [DataContract]
    public class DetailsObjetRisqueDelQueryDto : _DetailsObjetRisque_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the offre.
        /// </summary>
        /// <value>
        /// The offre.
        /// </value>
        [DataMember]
        public OffreDto offre { get; set; }
    }
}
