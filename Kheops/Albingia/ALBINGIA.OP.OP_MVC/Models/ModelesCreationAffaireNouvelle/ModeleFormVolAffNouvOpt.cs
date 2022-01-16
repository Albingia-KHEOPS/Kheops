using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleFormVolAffNouvOpt
    {
        public Int64 GuidOpt { get; set; }
        public Int64 CodeOpt { get; set; }
        public bool CheckRow { get; set; }
        public List<ModeleFormVolAffNouvVol> Volets { get; set; }

        public static explicit operator ModeleFormVolAffNouvOpt(FormVolAffNouvOptDto formVolAffNouvOptDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FormVolAffNouvOptDto, ModeleFormVolAffNouvOpt>().Map(formVolAffNouvOptDto);
        }

        public static FormVolAffNouvOptDto LoadDto(ModeleFormVolAffNouvOpt modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormVolAffNouvOpt, FormVolAffNouvOptDto>().Map(modele);
        }
    }
}