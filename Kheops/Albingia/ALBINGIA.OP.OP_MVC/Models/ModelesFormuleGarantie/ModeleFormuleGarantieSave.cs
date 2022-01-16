using EmitMapper;
using OP.WSAS400.DTO.FormuleGarantie;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    public class ModeleFormuleGarantieSave
    {
        public List<VoletSave> Volets { get; set; }


        public ModeleFormuleGarantieSave()
        {
            Volets = new List<VoletSave>();
        }

        public static FormuleGarantieSaveDto LoadDto(ModeleFormuleGarantieSave modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormuleGarantieSave, FormuleGarantieSaveDto>().Map(modele);
        }
    }
}