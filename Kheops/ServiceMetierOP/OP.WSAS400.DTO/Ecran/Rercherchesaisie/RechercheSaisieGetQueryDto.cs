using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Ecran.Rercherchesaisie
{
    [DataContract]
    public class RechercheSaisieGetQueryDto : _RechercheSaisie_Base, IQuery
    {
        /// <summary>
        /// Gets or sets the offres.
        /// </summary>
        /// <value>
        /// The offres.
        /// </value>
        [DataMember]
        public List<Offres.OffreDto> Offres {get; set;}
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public bool IsUserHorse { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RechercheSaisieGetQueryDto"/> class.
        /// </summary>
        public RechercheSaisieGetQueryDto()
        {
            this.Offres = new List<Offres.OffreDto>();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RechercheSaisieGetQueryDto"/> class.
        /// </summary>
        /// <param name="souscripteurs">The souscripteurs.</param>
        /// <param name="gestionnaires">The gestionnaires.</param>
        /// <param name="branches">The branches.</param>
        /// <param name="etats">The etats.</param>
        /// <param name="situations">The situations.</param>
        /// <param name="offres">The offres.</param>
        public RechercheSaisieGetQueryDto(List<Offres.OffreDto> offres)
        {
            this.Offres = offres;
        }
    }
}
