using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleLibellesClauses
{
    [Serializable]
    public class ModeleRechercheClauses
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Display(Name = "Description")]
        public string Description { get; set; }

        public ModeleRechercheClauses()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
        }
    }
}