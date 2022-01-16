using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleFormVolAffNouvRsq
    {
        public Int64 CodeRisque { get; set; }
        public string DescRisque { get; set; }
        public List<ModeleFormVolAffNouvForm> Formules { get; set; }


        public static explicit operator ModeleFormVolAffNouvRsq(FormVolAffNouvRsqDto formVolAffNouvRsqDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FormVolAffNouvRsqDto, ModeleFormVolAffNouvRsq>().Map(formVolAffNouvRsqDto);
        }

        public static FormVolAffNouvRsqDto LoadDto(ModeleFormVolAffNouvRsq modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormVolAffNouvRsq, FormVolAffNouvRsqDto>().Map(modele);
        }
    }
}