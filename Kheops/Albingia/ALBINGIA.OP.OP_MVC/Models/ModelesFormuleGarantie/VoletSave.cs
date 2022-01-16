using EmitMapper;
using OP.WSAS400.DTO.FormuleGarantie;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    public class VoletSave
    {
        public bool MAJ { get; set; }
        public int GuidId { get; set; }
        public bool isChecked { get; set; }
        public List<BlocSave> Blocs { get; set; }

        public static explicit operator VoletSave(VoletSaveDto dtoVolet)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<VoletSaveDto, VoletSave>().Map(dtoVolet);
        }
    }
}