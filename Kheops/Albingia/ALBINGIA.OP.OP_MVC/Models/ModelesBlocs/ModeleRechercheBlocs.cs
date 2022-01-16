using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesBlocs
{
    [Serializable]
    public class ModeleRechercheBlocs
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name="Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Display(Name="Description")]
        public string Description { get; set; }

        public ModeleRechercheBlocs()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
        }
    }
}