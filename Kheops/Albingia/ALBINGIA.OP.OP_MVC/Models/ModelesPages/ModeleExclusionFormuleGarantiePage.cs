using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleExclusionFormuleGarantiePage : MetaModelsBase
    {
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }

        public string RisqueCoche { get; set; }
        //public bool RisqueObjetCoche { get; set; }

        public ModeleFormuleGarantieLstObjRsq ObjetsRisques { get; set; }

        [Display(Name = "Formule : ")]
        public string LettreFormule { get; set; }
        [Display(Name = "Garantie : ")]
        public string Descriptif { get; set; }
        [Display(Name = "s'applique à")]
        public string ObjetRisque { get; set; }
        public string ObjetRisqueCode { get; set; }

        public ModeleExclusionFormuleGarantiePage()
        {
            this.CodeFormule = string.Empty;
            this.CodeOption = string.Empty;

            this.RisqueCoche = string.Empty;
            //this.RisqueObjetCoche = false;

            this.ObjetsRisques = new ModeleFormuleGarantieLstObjRsq();

            this.LettreFormule = string.Empty;
            this.Descriptif = string.Empty;
            this.ObjetRisque = string.Empty;
            this.ObjetRisqueCode = string.Empty;
        }
    }
}