using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ALBINGIA.Framework.Common.Extensions;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class InformationSaisie_MetaData : _SaisieCreation_MetaData_Base
    {
        [Required(ErrorMessage = "Veuillez choisir une branche.")]
        [Display(Name = "Branche *")]
        public String Branche { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }

        /// <summary>
        /// Contient la liste déroulante cible ainsi que la valeur selectionnée  
        /// </summary>
        /// <value>
        /// The information cible.
        /// </value>
        public InformationSaisie_MetaData_Cibles InformationCible { get; set; }

        public String CodeSouscripteur { get; set; }
        //[Required(ErrorMessage = "Veuillez choisir un souscripteur.")]
        [Display(Name = "Souscripteur")]
        public String Souscripteurs { get; set; }

        public String CodeGestionnaire { get; set; }
        //[Required(ErrorMessage = "Veuillez choisir un gestionnaire.")]
        [Display(Name = "Gestionnaire")]
        public String Gestionnaires { get; set; }

        public DateTime DateSaisie { get; set; }

        [Display(Name = "Date de saisie *")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [RegularExpression(@"^\d{2}/\d{2}/\d{4}$", ErrorMessage = "La date doit suivre la forme 24/11/2030")]
        public DateTime DateSaisieString
        {
            get { return DateSaisie; }
            set { DateSaisie = new DateTime(value.Year, value.Month, value.Day, DateSaisie.Hour, DateSaisie.Minute, 00); }
        }

        [Display(Name = "Heure de saisie")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [RegularExpression(@"^(([0-1]){1}([0-9]{1})|(2[0-3]))(:)([0-5]{1}[0-9]{1})$", ErrorMessage = "L'heure doit suivre la forme 16:32")]
        public TimeSpan HeureSaisieString
        {
            get { return new TimeSpan(DateSaisie.Hour, DateSaisie.Minute, DateSaisie.Second); }
            set { DateSaisie = new DateTime(DateSaisie.Year, DateSaisie.Month, DateSaisie.Day, value.Hours, value.Minutes, 00); }
        }
        //public DateTime HeureSaisieString
        //{
        //    get { return new DateTime(DateSaisie.Year, DateSaisie.Month, DateSaisie.Day, 0, 0, 00); }
        //    set { DateSaisie = new DateTime(DateSaisie.Year, DateSaisie.Month, DateSaisie.Day, value.Hour, value.Minute, 00); }
        //}

        public bool EditMode { get; set; }
        public bool CopyMode { get; set; }

        public InformationSaisie_MetaData()
        {
            this.Branches = new List<AlbSelectListItem>();
            this.InformationCible = new InformationSaisie_MetaData_Cibles();
            this.CodeSouscripteur = String.Empty;
            this.Souscripteurs = String.Empty;
            this.CodeGestionnaire = String.Empty;
            this.Gestionnaires = String.Empty;
            this.DateSaisie = DateTime.Now;
        }

    }
}