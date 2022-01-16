using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle;
using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleRsqObjAffNouvPage : MetaModelsBase
    {
        public string CodeOffre { get; set; }
        public Int64 Version { get; set; }
        public string Type { get; set; }
        public string CodeContrat { get; set; }
        public Int64 VersionContrat { get; set; }

        public List<ModeleRsqObjAffNouv> ListRsqObj { get; set; }

        public static explicit operator ModeleRsqObjAffNouvPage(RsqObjAffNouvDto rsqObjAffNouvDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RsqObjAffNouvDto, ModeleRsqObjAffNouvPage>().Map(rsqObjAffNouvDto);
        }

        public static RsqObjAffNouvDto LoadDto(ModeleRsqObjAffNouvPage modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleRsqObjAffNouvPage, RsqObjAffNouvDto>().Map(modele);
        }
    }
}