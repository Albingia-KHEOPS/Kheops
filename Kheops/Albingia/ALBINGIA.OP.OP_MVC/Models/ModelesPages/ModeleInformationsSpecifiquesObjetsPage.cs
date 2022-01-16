using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleInformationsSpecifiquesObjetsPage : MetaModelsBase
    {
        public string Code { get; set; }
        //[Display(Name = "Indice Réf.")]
        //public string IndiceRef { get; set; }
        //[Display(Name = "Valeur")]
        //public string Valeur { get; set; }
        //[Display(Name = "Risque Indexé")]
       // public bool RisqueIndexe { get; set; }

        [Display(Name = "Code risque")]
        public string CodeRisque { get; set; }
        [Display(Name = "Libellé risque")]
        public string LibelleRisque { get; set; }
        [Display(Name = "Code objet")]
        public string CodeObjet { get; set; }
        [Display(Name = "Libellé objet")]
        public string LibelleObjet { get; set; }

        public string AlbContext { get; set; }


        public bool IsDataRecup { get; set; }
    }
}