using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleOptTarAffNouvGaran
    {
        public Int64 CodeForm { get; set; }
        public string CodeGaran { get; set; }
        public string DescGaran { get; set; }
        public string LettreForm { get; set; }
        public List<ModeleOptTarAffNouvTarif> Tarifs { get; set; }

        public static explicit operator ModeleOptTarAffNouvGaran(OptTarAffNouvGaranDto optTarAffNouvGaranDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<OptTarAffNouvGaranDto, ModeleOptTarAffNouvGaran>().Map(optTarAffNouvGaranDto);
        }

        public static OptTarAffNouvGaranDto LoadDto(ModeleOptTarAffNouvGaran modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleOptTarAffNouvGaran, OptTarAffNouvGaranDto>().Map(modele);
        }
    }
}