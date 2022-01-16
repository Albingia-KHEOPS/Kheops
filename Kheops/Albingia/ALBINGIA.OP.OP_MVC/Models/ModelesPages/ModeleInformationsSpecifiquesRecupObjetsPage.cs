using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleInformationsSpecifiquesRecupObjetsPage: MetaModelsBase
    {
        public string Code { get; set; }

        [Display(Name = "Code risque")]
        public string CodeRisque { get; set; }
        [Display(Name = "Libellé risque")]
        public string LibelleRisque { get; set; }
        [Display(Name = "Code objet")]
        public string CodeObjet { get; set; }
        [Display(Name = "Libellé objet")]
        public string LibelleObjet { get; set; }

        public string AlbContext { get; set; }
      


    }   
}