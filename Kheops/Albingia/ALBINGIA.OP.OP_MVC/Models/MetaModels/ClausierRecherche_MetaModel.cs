using EmitMapper;
using OP.WSAS400.DTO.Clause;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    public class ClausierRecherche_MetaModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        /// <value>
        /// The designation.
        /// </value>
        public string Titre { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string NumeroVersion { get; set; }

        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        public string Branche { get; set; }

        /// <summary>
        /// Gets or sets the historique.
        /// </summary>
        /// <value>
        /// The historique.
        /// </value>
        public List<ClausierHistoriqueMetaModel> Historique { get; set; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="ALBINGIA.OP.Layers.Services.WS_AS400.ClauseDto"/> to <see cref="ALBINGIA.OP.OP_MVC.Models.MetaModels.ClausierRecherche_MetaModel"/>.
        /// </summary>
        /// <param name="dtoClausier">The dto clausier.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator ClausierRecherche_MetaModel(ClauseDto dtoClausier)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ClauseDto, ClausierRecherche_MetaModel>().Map(dtoClausier);
        }

        public ClausierRecherche_MetaModel()
        {
            Historique = new List<ClausierHistoriqueMetaModel>();
        }
    }  
}
