using EmitMapper;
using OP.WSAS400.DTO.Traite;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTraites
{
    [Serializable]
    public class ModeleTraiteInfoRsqVen
    {
        public List<ModeleTraiteRisque> TraiteRisques { get; set; }
        public List<ModeleTraiteVentilation> TraiteVentilations { get; set; }

        public static explicit operator ModeleTraiteInfoRsqVen(TraiteInfoRsqVenDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TraiteInfoRsqVenDto, ModeleTraiteInfoRsqVen>().Map(modelDto);
        }

        public static TraiteInfoRsqVenDto LoadDto(ModeleTraiteInfoRsqVen model)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleTraiteInfoRsqVen, TraiteInfoRsqVenDto>().Map(model);
        }
    }
}