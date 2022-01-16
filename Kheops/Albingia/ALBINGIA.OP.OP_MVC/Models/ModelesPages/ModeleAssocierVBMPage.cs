using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleCibles;
using ALBINGIA.OP.OP_MVC.Models.ModelesBlocs;
using ALBINGIA.OP.OP_MVC.Models.ModelesCategories;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieModeles;
using ALBINGIA.OP.OP_MVC.Models.ModelesVolets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleAssocierVBMPage : MetaModelsBase
    {
        /// <summary>
        /// Gets or sets the branche.
        /// </summary>
        /// <value>
        /// The branche.
        /// </value>
        [Display(Name = "Branche")]
        public string Branche { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        public List<ModeleCible> Cibles { get; set; }
        public List<ModeleCategorie> Categories { get; set; }
        public List<ModeleVolet> Volets { get; set; }
        public List<ModeleBloc> Blocs { get; set; }
        public List<ModeleGarantieModele> GarantieModeles { get; set; }

        /// <summary>
        /// Gets or sets the volet.
        /// </summary>
        /// <value>
        /// The volet.
        /// </value>
        [Display(Name = "Code, Description")]
        public string Volet { get; set; }
        public List<AlbSelectListItem> VoletsList { get; set; }
        /// <summary>
        /// Gets or sets the bloc.
        /// </summary>
        /// <value>
        /// The bloc.
        /// </value>
        [Display(Name = "Code, Description")]
        public string Bloc { get; set; }
        public List<AlbSelectListItem> BlocsList { get; set; }
        /// <summary>
        /// Gets or sets the caractere.
        /// </summary>
        /// <value>
        /// The caractere.
        /// </value>
        [Display(Name = "Carac.")]
        public string CaractereVolet { get; set; }
        public List<AlbSelectListItem> CaracteresVolet { get; set; }
        /// <summary>
        /// Gets or sets the caractere.
        /// </summary>
        /// <value>
        /// The caractere.
        /// </value>
        [Display(Name = "Carac.")]
        public string CaractereBloc { get; set; }
        public List<AlbSelectListItem> CaracteresBloc { get; set; }

        /// <summary>
        /// Gets or sets the modele.
        /// </summary>
        /// <value>
        /// The modele.
        /// </value>
        [Display(Name = "Code, Description")]
        public string Modele { get; set; }
        public List<AlbSelectListItem> ModelesList { get; set; }
        /// <summary>
        /// Gets or sets the date appli.
        /// </summary>
        /// <value>
        /// The date appli.
        /// </value>
        [Display(Name = "Date Appli")]
        public DateTime? DateAppli { get; set; }
        /// <summary>
        /// Gets or sets the typologie.
        /// </summary>
        /// <value>
        /// The typologie.
        /// </value>
        [Display(Name = "Typo.")]
        public string Typologie { get; set; }
        public List<AlbSelectListItem> TypologiesList { get; set; }


        public ModeleAssocierVBMPage()
        {
            this.Branche = string.Empty;
            this.Branches = new List<AlbSelectListItem>();

            this.Categories = new List<ModeleCategorie>();
            this.Volets = new List<ModeleVolet>();
            this.Blocs = new List<ModeleBloc>();
            this.GarantieModeles = new List<ModeleGarantieModele>();

            this.Volet = string.Empty;
            this.VoletsList = new List<AlbSelectListItem>();
            this.Bloc = string.Empty;
            this.BlocsList = new List<AlbSelectListItem>();
            this.CaractereVolet = string.Empty;
            this.CaracteresVolet = new List<AlbSelectListItem>();
            this.CaractereBloc = string.Empty;
            this.CaracteresBloc = new List<AlbSelectListItem>();

            this.Modele = string.Empty;
            this.ModelesList = new List<AlbSelectListItem>();
            this.DateAppli = null;
            this.Typologie = string.Empty;
            this.TypologiesList = new List<AlbSelectListItem>();
        }
    }
}