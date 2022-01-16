using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocs;
using ALBINGIA.OP.OP_MVC.Models.ModelesCategories;
using EmitMapper;
using OP.WSAS400.DTO.Volet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesVolets
{
    [Serializable]
    public class ModeleVolet
    {
        public bool ReadOnly { get; set; }
        public string GuidId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code Volet")]
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }
        public string Caractere { get; set; }
        public List<AlbSelectListItem> Caracteres { get; set; }
        public bool isChecked { get; set; }
        //public bool IsChecked
        //{
        //    get {
        //        return Caractere != "S";
        //    }
        //}
        /// <summary>
        /// Gets or sets the date creation.
        /// </summary>
        /// <value>
        /// The date creation.
        /// </value>
        [Display(Name = "Créé le")]
        public DateTime? DateCreation { get; set; }

        public List<ModeleCategorie> CategoriesVolet { get; set; }
        public string Categorie { get; set; }
        public List<AlbSelectListItem> Categories { get; set; }
        public List<AlbSelectListItem> Branche { get; set; }

        public List<ModeleBloc> Blocs { get; set; }

        public string CodeOption { get; set; }

        public bool MAJ { get; set; }

        public string IsVoletGeneral { get; set; }

        public double OrdreVolet { get; set; }

        public bool ModifAvt { get; set; }
        public bool IsReadOnly { get; set; }
        public bool ModeAvt { get; set; }

        public List<AlbSelectListItem> ParamNatMods { get; set; }

        public string IsVoletCollapse { get; set; }

        public ModeleVolet()
        {
            Code = string.Empty;
            Description = string.Empty;
            Caractere = string.Empty;
            Caracteres = new List<AlbSelectListItem>();
            DateCreation = null;
            ReadOnly = false;
            GuidId = string.Empty;

            CategoriesVolet = new List<ModeleCategorie>();
            Categorie = string.Empty;
            Categories = new List<AlbSelectListItem>();

            Blocs = new List<ModeleBloc>();

            CodeOption = string.Empty;
            isChecked = false;
        }

        public static explicit operator ModeleVolet(DtoVolet dtoVolet)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<DtoVolet, ModeleVolet>().Map(dtoVolet);
        }


    }
}