using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.DetailsRisque_MetaData
{
    public class InformationsGenerales_MetaData : _DetailsRisque_MetaData_Base
    {
        public bool ReadOnly { get; set; }
        public bool MultiObjet { get; set; }

        [Display(Name = "Risque")]
        public string DescRisque { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Display(Name = "Code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the descriptif.
        /// </summary>
        /// <value>
        /// The descriptif.
        /// </value>
        [Display(Name = "Descriptif *")]
        [Required]
        public String Descriptif { get; set; }

        /// <summary>
        /// Gets or sets the designation.
        /// </summary>
        /// <value>
        /// The designation.
        /// </value>
        [Display(Name = "Désignation")]
        public String Designation { get; set; }

        [Display(Name = "Cible *")]
        [Required]
        public String Cible { get; set; }
        /// <summary>
        /// Gets or sets the cibles.
        /// </summary>
        /// <value>
        /// The cibles.
        /// </value>
        public List<AlbSelectListItem> Cibles { get; set; }

        public DateTime? DateDebEffet { get; set; }
        public DateTime? DateFinEffet { get; set; }

        /// <summary>
        /// Gets or sets the date entree garantie.
        /// </summary>
        /// <value>
        /// The date entree garantie.
        /// </value>
        [Display(Name = "Entrée")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateEntreeGarantie { get; set; }

        [Display(Name = "Heure entrée")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureEntreeGarantie { get; set; }

        /// <summary>
        /// Gets or sets the date sortie garantie.
        /// </summary>
        /// <value>
        /// The date sortie garantie.
        /// </value>
        [Display(Name = "Sortie")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime? DateSortieGarantie { get; set; }

        [Display(Name = "Heure sortie")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan? HeureSortieGarantie { get; set; }

        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        [Display(Name = "Valeur")]
        public int? Valeur { get; set; }

        [Display(Name = "Unité")]
        public String Unite { get; set; }
        /// <summary>
        /// Gets or sets the unites.
        /// </summary>
        /// <value>
        /// The unites.
        /// </value>
        public List<AlbSelectListItem> Unites { get; set; }

        [Display(Name = "Type")]
        public String Type { get; set; }
        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        public List<AlbSelectListItem> Types { get; set; }

        [Display(Name = "HT/TTC")]
        public String ValeurHT { get; set; }
        public List<AlbSelectListItem> ValeursHT { get; set; }

        public bool DisplayInfosValeur { get; set; }

        public bool IsReadOnly { get; set; }

        public InformationsGenerales_MetaData()
        {
            this.DisplayInfosValeur = true;
            this.ReadOnly = false;
            this.Descriptif = string.Empty;
            this.Designation = string.Empty;
            this.Cibles = new List<AlbSelectListItem>();
            this.DateEntreeGarantie = null;
            this.HeureEntreeGarantie = null;
            this.DateSortieGarantie = null;
            this.HeureSortieGarantie = null;
            this.Valeur = null;
            this.Unites = new List<AlbSelectListItem>();
            this.Types = new List<AlbSelectListItem>();
        }
    }
}