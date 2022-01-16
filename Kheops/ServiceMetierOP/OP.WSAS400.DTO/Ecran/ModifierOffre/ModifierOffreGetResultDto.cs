using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Parametres;
using OP.WSAS400.DTO.Personnes;


namespace OP.WSAS400.DTO.Ecran.ModifierOffre
{
    [DataContract]
    public class ModifierOffreGetResultDto //: _ModifierOffre_Base, IResult
    {
        [DataMember]
        public OffreDto Offre { get; set; }

        /// <summary>
        /// Gets or sets the mots cles.
        /// </summary>
        /// <value>
        /// The mots cles.
        /// </value>
        [DataMember]
        public List<ParametreDto> MotsCles { get; set; }

        /// <summary>
        /// Gets or sets the periodicites.
        /// </summary>
        /// <value>
        /// The periodicites.
        /// </value>
        [DataMember]
        public List<ParametreDto> Periodicites { get; set; }

        /// <summary>
        /// Gets or sets the devises.
        /// </summary>
        /// <value>
        /// The devises.
        /// </value>
        [DataMember]
        public List<ParametreDto> Devises { get; set; }

        /// <summary>
        /// Gets or sets the indices.
        /// </summary>
        /// <value>
        /// The indices.
        /// </value>
        [DataMember]
        public List<ParametreDto> Indices { get; set; }

        /// <summary>
        /// Gets or sets the natures contrat.
        /// </summary>
        /// <value>
        /// The natures contrat.
        /// </value>
        [DataMember]
        public List<ParametreDto> NaturesContrat { get; set; }

        /// <summary>
        /// Gets or sets the aperiteurs.
        /// </summary>
        /// <value>
        /// The aperiteurs.
        /// </value>
        [DataMember]
        public List<ParametreDto> Aperiteurs { get; set; }

        /// <summary>
        /// Gets or sets the souscripteurs.
        /// </summary>
        /// <value>
        /// The souscripteurs.
        /// </value>
        [DataMember]
        public List<SouscripteurDto> Souscripteurs { get; set; }

        /// <summary>
        /// Gets or sets the gestionnaires.
        /// </summary>
        /// <value>
        /// The gestionnaires.
        /// </value>
        [DataMember]
        public List<GestionnaireDto> Gestionnaires { get; set; }

        /// <summary>
        /// Gets or sets the durees.
        /// </summary>
        /// <value>
        /// The durees.
        /// </value>
        [DataMember]
        public List<ParametreDto> Durees { get; set; }
        /// <summary>
        /// Gets or sets the taxes regime.
        /// </summary>
        /// <value>
        /// The durees.
        /// </value>
        [DataMember]
        public List<ParametreDto> RegimesTaxe { get; set; }

        [DataMember]
        public List<ParametreDto> Antecedents { get; set; }
        [DataMember]
        public List<ParametreDto> Stops { get; set; }
    }
}
