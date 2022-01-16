using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie
{
    public class ModelePreneurAssurance
    {
        [Display(Name = "Code *")]
        public int? CodePreneurAssurance { get; set; }

        [Display(Name = "Nom")]
        public string NomPreneurAssurance { get; set; }

        [Display(Name = "Dépt")]
        public string Departement { get; set; }

        [Display(Name = "Ville")]
        public string Ville { get; set; }

        public bool PreneurEstAssure { get; set; }

        public bool EditMode { get; set; }

        public bool IsReadOnlyDisplay { get; set; }
        public Int64 NbAssuAdditionnel { get; set; }

    }
}