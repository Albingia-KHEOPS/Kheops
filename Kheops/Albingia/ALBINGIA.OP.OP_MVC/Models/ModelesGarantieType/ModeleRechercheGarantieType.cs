using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesGarantieType
{
    [Serializable]
    public class ModeleRechercheGarantieType
    {
        /// <summary>
        /// Gets or sets the code modele.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code Modèle de garantie")]
        public string CodeModele { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code")]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the niveau.
        /// </summary>
        /// <value>
        /// The niveau.
        /// </value>
        [Display(Name = "Niveau")]
        public int Niveau { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModeleRechercheGarantieModele"/> class.
        /// </summary>
        public ModeleRechercheGarantieType()
        {
            this.CodeModele = string.Empty;
            this.Code = string.Empty;
            this.Niveau = 0;
        }
    }
}