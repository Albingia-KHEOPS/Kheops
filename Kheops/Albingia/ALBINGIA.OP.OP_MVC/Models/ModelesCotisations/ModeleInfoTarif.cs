using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCotisations
{
    [Serializable]
    public class ModeleInfoTarif
    {
        public string NomGarantie { get; set; }
        public List<ModeleTarif> Tarifs { get; set; }

        public static explicit operator ModeleInfoTarif(CotisationsInfoTarifDto InfoTarifDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CotisationsInfoTarifDto, ModeleInfoTarif>().Map(InfoTarifDto);
        }

        public static CotisationsInfoTarifDto LoadDto(ModeleInfoTarif modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleInfoTarif, CotisationsInfoTarifDto>().Map(modele);
        }
    }
}