using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.ModeleModeles;
using EmitMapper;
using OP.WSAS400.DTO.Bloc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesBlocs
{
    [Serializable]
    public class ModeleBloc
    {
        public int IdLigneVide { get; set; }
        public bool ReadOnly { get; set; }
        public string GuidId { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code Bloc")]
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
        public bool isChecked { get; set; }
      
        /// <summary>
        /// Gets or sets the date creation.
        /// </summary>
        /// <value>
        /// The date creation.
        /// </value>
        [Display(Name = "Créé le")]
        public DateTime? DateCreation { get; set; }

        public List<ModeleModele> Modeles { get; set; }
        public List<ModeleLigneBloc> ListeBlocsIncompatibles { get; set; }
        public List<ModeleLigneBloc> ListeBlocsAssocies { get; set; }
        public List<AlbSelectListItem> ListeReferentielBlocsIncompatibles { get; set; }
        public List<AlbSelectListItem> ListeReferentielBlocsAssocies { get; set; }


        public string CodeOption { get; set; }

        public bool MAJ { get; set; }

        public string BlocIncompatible { get; set; }
        public string BlocDependant { get; set; }
        public string BlocAssocie { get; set; }
        public string BlocAlternatif { get; set; }

        public double OrdreBloc { get; set; }

        public bool ModifAvt { get; set; }
        public bool IsReadOnly { get; set; }
        public bool ModeAvt { get; set; }

        public ModeleBloc()
        {
            this.Code = string.Empty;
            this.Description = string.Empty;
            this.Caractere = string.Empty;
            this.DateCreation = null;
            this.ReadOnly = false;
            this.GuidId = string.Empty;

            this.Modeles = new List<ModeleModele>();
            this.ListeBlocsIncompatibles = new List<ModeleLigneBloc>();
            this.ListeBlocsAssocies = new List<ModeleLigneBloc>();
            this.ListeReferentielBlocsAssocies = new List<AlbSelectListItem>();
            this.ListeReferentielBlocsIncompatibles = new List<AlbSelectListItem>();
            //this.Modeles = new List<ModeleGarantieNiveau1>();

            this.CodeOption = string.Empty;
            this.isChecked = false;
        }

        public static explicit operator ModeleBloc(BlocDto BlocDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<BlocDto, ModeleBloc>().Map(BlocDto);
        }
    }
}