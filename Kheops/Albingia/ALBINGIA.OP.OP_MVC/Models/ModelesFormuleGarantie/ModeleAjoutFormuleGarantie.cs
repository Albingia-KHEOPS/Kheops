using ALBINGIA.OP.OP_MVC.Models.MetaData;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    [Serializable]
    public class ModeleAjoutFormuleGarantie : MetaModelsBase
    {
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string CodeGarantie { get; set; }

        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }

        public ModeleFormuleGarantieLstObjRsq ObjetsRisques { get; set; }

        [Display(Name = "Formule")]
        public string LettreFormule { get; set; }
        [Display(Name = "Garantie")]
        public string Descriptif { get; set; }
        [Display(Name = "s'applique à")]
        public string ObjetRisque { get; set; }
        public string ObjetRisqueCode { get; set; }

        public ListInventaires_MetaData Inventaires { get; set; }

        public ModeleAjoutFormuleGarantie()
        {
            this.CodeOffre = string.Empty;
            this.Version = string.Empty;
            this.CodeGarantie = string.Empty;
            this.CodeFormule = string.Empty;
            this.CodeOption = string.Empty;
            this.ObjetsRisques = new ModeleFormuleGarantieLstObjRsq();
            this.LettreFormule = string.Empty;
            this.Descriptif = string.Empty;
            this.ObjetRisque = string.Empty;
            this.ObjetRisqueCode = string.Empty;
            this.Inventaires = new ListInventaires_MetaData();
        }

        //public static explicit operator ModeleAjoutFormuleGarantie(FormuleGarantieDetailsDto FormuleDto)
        //{
        //    return = ObjectMapperManager.DefaultInstance.GetMapper<FormuleGarantieDetailsDto, ModeleAjoutFormuleGarantie>().Map(FormuleDto);
        //}

        //public static FormuleGarantieDetailsDto LoadDto(ModeleAjoutFormuleGarantie modele)
        //{
        //    return ObjectMapperManager.DefaultInstance.GetMapper<ModeleAjoutFormuleGarantie, FormuleGarantieDetailsDto>().Map(modele);
        //}

    }
}