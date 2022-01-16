using EmitMapper;
using OP.WSAS400.DTO.Categorie;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCategories
{
    [Serializable]
    public class ModeleCategorie
    {
        public bool ReadOnly { get; set; }
        public string GuidId { get; set; }

           /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name="Code Catégorie")]
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Display(Name="Description")]
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the date creation.
        /// </summary>
        /// <value>
        /// The date creation.
        /// </value>
        [Display(Name="Créé le")]
        public DateTime? DateCreation { get; set; }
        /// <summary>
        /// Gets or sets the code branche.
        /// </summary>
        /// <value>
        /// The code branche.
        /// </value>
        [Display(Name="Branche")]
        public string CodeBranche { get; set; }
        /// <summary>
        /// Gets or sets the caractere.
        /// </summary>
        /// <value>
        /// The caractere.
        /// </value>
        [Display(Name="Caractère")]
        public string Caractere { get; set; }


        public ModeleCategorie()
        {
            GuidId = string.Empty;
            Code = string.Empty;
            Description = string.Empty;
            DateCreation = null;
            CodeBranche = string.Empty;
            ReadOnly = false;
        }

        public static explicit operator ModeleCategorie(CategorieDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CategorieDto, ModeleCategorie>().Map(modeleDto);
        }

 }
}