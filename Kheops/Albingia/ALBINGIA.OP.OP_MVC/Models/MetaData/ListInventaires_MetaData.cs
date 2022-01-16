using ALBINGIA.Framework.Common.Extensions;
//using ALBINGIA.OP.OP_MVC.Models.MetaModels.DetailsRisque_MetaModels;
using ALBINGIA.OP.OP_MVC.Models.MetaModels.DetailsObjet_MetaModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.MetaData
{
    public class ListInventaires_MetaData : ModelBase
    {

        /// <summary>
        /// Gets or sets the code inventaire.
        /// </summary>
        /// <value>
        /// The code inventaire.
        /// </value>
        public string CodeInventaire { get; set; }

        /// <summary>
        /// Gets or sets the type inventaire.
        /// </summary>
        /// <value>
        /// The type inventaire.
        /// </value>
        [Display(Name="Type inventaire")]
        public String TypeInventaire { get; set; }

        /// <summary>
        /// Gets or sets the types inventaire.
        /// </summary>
        /// <value>
        /// The types inventaire.
        /// </value>
        public List<AlbSelectListItem> TypesInventaire { get; set; }

        /// <summary>
        /// Gets or sets the inventaires.
        /// </summary>
        /// <value>
        /// The inventaires.
        /// </value>
        public DetailsObjet_Inventaires_MetaModel Inventaires { get; set; }

        public string TypeList { get; set; }

        public bool IsReadOnly { get; set; }

        public ListInventaires_MetaData()
        {
            this.CodeInventaire = string.Empty;
            this.TypeInventaire = string.Empty;
            this.TypesInventaire = new List<AlbSelectListItem>();
            this.Inventaires = new DetailsObjet_Inventaires_MetaModel();
            this.TypeList = string.Empty;
        }
    }
}