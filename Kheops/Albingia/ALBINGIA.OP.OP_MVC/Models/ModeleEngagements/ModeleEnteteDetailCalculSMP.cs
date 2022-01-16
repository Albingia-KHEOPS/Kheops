using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleEngagements
{
    [Serializable]
    public class ModeleEnteteDetailCalculSMP
    {
        [Display(Name="Nom du traité")]
        public string NomTraite { get; set; }
        [Display(Name = "Risque")]
        public string Risque { get; set; }
        [Display(Name = "Ventilation")]
        public string Ventilation { get; set; }
    }
}