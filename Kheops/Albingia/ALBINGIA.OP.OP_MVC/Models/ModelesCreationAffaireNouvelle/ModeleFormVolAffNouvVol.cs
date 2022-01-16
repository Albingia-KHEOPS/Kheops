using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleFormVolAffNouvVol
    {
        public Int64 GuidVolet { get; set; }
        public string CodeVolet { get; set; }
        public string DescVolet { get; set; }
        public bool CheckRow { get; set; }

        public static explicit operator ModeleFormVolAffNouvVol(FormVolAffNouvVolDto formVolAffNouvVolDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FormVolAffNouvVolDto, ModeleFormVolAffNouvVol>().Map(formVolAffNouvVolDto);
        }

        public static FormVolAffNouvVolDto LoadDto(ModeleFormVolAffNouvVol modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormVolAffNouvVol, FormVolAffNouvVolDto>().Map(modele);
        }
    }
}