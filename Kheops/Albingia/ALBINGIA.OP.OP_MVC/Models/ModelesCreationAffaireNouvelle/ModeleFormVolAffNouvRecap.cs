using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleFormVolAffNouvRecap
    {
        public string CodeContrat { get; set; }
        public string VersionContrat { get; set; }
        public List<ModeleFormVolAffNouvRsq> Risques { get; set; }
        public List<ModeleFormVolAffNouvForm> Formules { get; set; }
        public Int64 CountGarTar { get; set; }

        public static explicit operator ModeleFormVolAffNouvRecap(FormVolAffNouvRecapDto formVolAffNouvRecapDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<FormVolAffNouvRecapDto, ModeleFormVolAffNouvRecap>().Map(formVolAffNouvRecapDto);
        }

        public static FormVolAffNouvRecapDto LoadDto(ModeleFormVolAffNouvRecap modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleFormVolAffNouvRecap, FormVolAffNouvRecapDto>().Map(modele);
        }
    }
}