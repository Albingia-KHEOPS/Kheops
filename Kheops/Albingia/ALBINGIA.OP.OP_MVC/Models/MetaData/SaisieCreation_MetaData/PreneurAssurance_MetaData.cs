using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.MetaData.SaisieCreation_MetaData
{
    public class PreneurAssurance_MetaData : _SaisieCreation_MetaData_Base
    {
        [Display(Name = "Code")]
        //[Required(ErrorMessage = "Veuillez choisir un preneur d'assurance.")]
        public int? CodePreneurAssurance { get; set; }

        //[Display(Name = "Nom du preneur d'assurance")]
        [Display(Name = "Nom du preneur")]
        public string NomPreneurAssurance { get; set; }

        [Display(Name = "Dépt")]
        public string Departement { get; set; }

        [Display(Name = "Ville")]
        public string Ville { get; set; }

        public PreneurAssurance_MetaData()
        {
            this.CodePreneurAssurance = null;
            this.NomPreneurAssurance = String.Empty;
            this.Departement = String.Empty;
            this.Ville = String.Empty;
        }
    }
}