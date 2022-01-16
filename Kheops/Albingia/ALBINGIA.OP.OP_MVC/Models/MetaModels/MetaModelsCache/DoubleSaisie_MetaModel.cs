using OP.WSAS400.DTO.Offres;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels.MetaModelsCache
{
    public class DoubleSaisie_MetaModel 
    {
        /// <summary>
        /// Gets or sets the double saisie autres.
        /// </summary>
        /// <value>
        /// The double saisie autres.
        /// </value>
        public List<OffreDto> DblSaisieAutresOffres { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus gestionnaire.
        /// </summary>
        /// <value>
        /// The motifs refus gestionnaire.
        /// </value>
        public List<ParametreDto> MotifsRefusGestionnaire { get; set; }

        /// <summary>
        /// Gets or sets the motifs refus demandeur.
        /// </summary>
        /// <value>
        /// The motifs refus demandeur.
        /// </value>
        public List<ParametreDto> MotifsRefusDemandeur { get; set; }
    }
}
