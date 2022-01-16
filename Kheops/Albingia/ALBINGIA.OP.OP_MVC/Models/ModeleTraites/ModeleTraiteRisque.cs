using EmitMapper;
using OP.WSAS400.DTO.Traite;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleTraites
{
    [Serializable]
    public class ModeleTraiteRisque
    {
        public int CodeRisque { get; set; }
        public string DescrRsq { get; set; }
        public List<ModeleTraiteVentilation> TraiteVentilations { get; set; }

        public static explicit operator ModeleTraiteRisque(TraiteRisqueDto TraiteRisqueDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TraiteRisqueDto, ModeleTraiteRisque>().Map(TraiteRisqueDto);
        }

        public static TraiteRisqueDto LoadDto(ModeleTraiteRisque modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleTraiteRisque, TraiteRisqueDto>().Map(modele);
        }

    
    }
}