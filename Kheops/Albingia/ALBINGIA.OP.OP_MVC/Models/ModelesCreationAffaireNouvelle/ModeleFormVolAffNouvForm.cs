using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleFormVolAffNouvForm
    {
        public Int64 GuidForm { get; set; }
        public Int64 CodeForm { get; set; }
        public string LettreForm { get; set; }
        public string DescFormule { get; set; }
        public bool CheckRow { get; set; }
        public List<ModeleFormVolAffNouvOpt> Options { get; set; }

        public static explicit operator ModeleFormVolAffNouvForm(FormVolAffNouvFormDto formVolAffNouvFormDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FormVolAffNouvFormDto, ModeleFormVolAffNouvForm>().Map(formVolAffNouvFormDto);
        }

        public static FormVolAffNouvFormDto LoadDto(ModeleFormVolAffNouvForm modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormVolAffNouvForm, FormVolAffNouvFormDto>().Map(modele);
        }
    }
}