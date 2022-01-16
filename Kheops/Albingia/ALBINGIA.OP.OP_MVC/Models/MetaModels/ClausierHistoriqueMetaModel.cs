using System;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    public class ClausierHistoriqueMetaModel
    {
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        /// <value>
        /// The numero.
        /// </value>
        public string Numero { get; set; }
        /// <summary>
        /// Gets or sets the date debut.
        /// </summary>
        /// <value>
        /// The date debut.
        /// </value>
        public DateTime DateDebut { get; set; }
        /// <summary>
        /// Gets or sets the date fin.
        /// </summary>
        /// <value>
        /// The date fin.
        /// </value>
        public DateTime DateFin { get; set; }

        //public static explicit operator ClausierHistoriqueMetaModel(ClauseVersionDto dtoClauseVersions)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<ClauseVersionDto, ClausierHistoriqueMetaModel>().Map(dtoClauseVersions);
        //}
    }
}