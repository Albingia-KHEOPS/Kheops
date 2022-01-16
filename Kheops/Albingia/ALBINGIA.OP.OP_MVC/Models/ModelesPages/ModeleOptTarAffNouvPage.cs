using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleOptTarAffNouvPage : MetaModelsBase
    {
        public string CodeOffre { get; set; }
        public Int64 Version { get; set; }
        public string CodeContrat { get; set; }
        public Int64 VersionContrat { get; set; }
        public List<ModeleOptTarAffNouvGaran> Garanties { get; set; }


        public static explicit operator ModeleOptTarAffNouvPage(OptTarAffNouvDto formVolAffNouvDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<OptTarAffNouvDto, ModeleOptTarAffNouvPage>().Map(formVolAffNouvDto);
        }

        public static OptTarAffNouvDto LoadDto(ModeleOptTarAffNouvPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleOptTarAffNouvPage, OptTarAffNouvDto>().Map(modele);
        }
    }
}