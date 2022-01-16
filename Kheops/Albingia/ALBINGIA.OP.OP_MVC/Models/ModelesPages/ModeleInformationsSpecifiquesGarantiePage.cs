using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleInformationsSpecifiquesGarantiePage : MetaModelsBase
    {
        [Display(Name = "Formule")]
        public string CodeFormule { get; set; }
        [Display(Name = "Code Option")]
        public string CodeOption { get; set; }
        [Display(Name = "Libellé")]
        public string LibelleFormule { get; set; }
        [Display(Name = "Lettre")]
        public string LettreLibelleFormule { get; set; }
        public string AlbContext { get; set; }
        public string CodeRisque { get; set; }

        public bool IsDataRecup { get; set; }
    }
}