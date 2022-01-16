using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie
{
    public class ModeleCabinetCourtage
    {
        [Display(Name="Code *")]
        public int? CodeCabinetCourtage { get; set; }

        [Display(Name = "Nom")]
        public string NomCabinetCourtage { get; set; }

        public string CodeInterlocuteur { get; set; }

        [Display(Name = "Interlocuteur")]
        public string NomInterlocuteur { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Réf.")]
        public string Reference { get; set; }

        public bool EditMode { get; set; }
        public bool CopyMode { get; set; }

        public bool IsReadOnlyDisplay { get; set; }
    }
}