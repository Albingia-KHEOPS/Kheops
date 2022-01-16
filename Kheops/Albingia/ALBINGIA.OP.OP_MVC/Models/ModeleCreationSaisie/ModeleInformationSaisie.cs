using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie
{
    public class ModeleInformationSaisie
    {
        [Required(ErrorMessage = "Veuillez choisir une branche.")]
        [Display(Name = "Branche *")]
        public String Branche { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        public ModeleInfoSaisieCible InformationCible { get; set; }
        public ModeleInfoSaisieTemplate InformationTemplate { get; set; }

      
        public String CodeSouscripteur { get; set; }
        [Display(Name = "Souscripteur *")]
        public String Souscripteurs { get; set; }

        public String CodeGestionnaire { get; set; }
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

        public bool EditMode { get; set; }
        public bool CopyMode { get; set; }

        /// <summary>
        /// Permet de savoir si la page est chargée dynamiquement sur changement de template dans la liste
        /// </summary>
        public bool LoadTemplateMode { get; set; }

        public bool IsConfirmation { get; set; }

        public bool IsReadOnlyDisplay { get; set; }

        public ModeleInformationSaisie()
        {
            Branches = new List<AlbSelectListItem>();
            InformationCible = new ModeleInfoSaisieCible();
            InformationTemplate = new ModeleInfoSaisieTemplate();
            DateSaisie = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            
        }
    }
}