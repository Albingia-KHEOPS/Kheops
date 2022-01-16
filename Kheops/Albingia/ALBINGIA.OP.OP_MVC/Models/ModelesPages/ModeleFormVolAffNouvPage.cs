using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleFormVolAffNouvPage : MetaModelsBase
    {
        public string CodeOffre { get; set; }
        public Int64 Version { get; set; }
        public string Type { get; set; }
        public string CodeContrat { get; set; }
        public Int64 VersionContrat { get; set; }

        public List<ModeleFormVolAffNouvRsq> Risques { get; set; }

        public static explicit operator ModeleFormVolAffNouvPage(FormVolAffNouvDto formVolAffNouvDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FormVolAffNouvDto, ModeleFormVolAffNouvPage>().Map(formVolAffNouvDto);
        }

        public static FormVolAffNouvDto LoadDto(ModeleFormVolAffNouvPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormVolAffNouvPage, FormVolAffNouvDto>().Map(modele);
        }
    }
}