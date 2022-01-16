using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleRsqObjAffNouv
    {
        public Int64 CodeRsq { get; set; }
        public Int64 CodeObj { get; set; }
        public string Libelle { get; set; }
        public bool CheckRow { get; set; }
        public string TypeEnr { get; set; }


        public static explicit operator ModeleRsqObjAffNouv(RsqObjAffNouvRowDto rsqObjAffNouvRowDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RsqObjAffNouvRowDto, ModeleRsqObjAffNouv>().Map(rsqObjAffNouvRowDto);
        }

        public static RsqObjAffNouvRowDto LoadDto(ModeleRsqObjAffNouv modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleRsqObjAffNouv, RsqObjAffNouvRowDto>().Map(modele);
        }
    }
}