using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCreationAffaireNouvelle
{
    [Serializable]
    public class ModeleOptTarAffNouvTarif
    {
        public Int64 CodeTarif { get; set; }
        public Int64 GuidTarif { get; set; }
        public decimal LCIVal { get; set; }
        public string LCIUnit { get; set; }
        public string LCIType { get; set; }
        public Int64 IdLCICpx { get; set; }
        public decimal FRHVal { get; set; }
        public string FRHUnit { get; set; }
        public string FRHType { get; set; }
        public Int64 IdFRHCpx { get; set; }
        public decimal ASSVal { get; set; }
        public string ASSUnit { get; set; }
        public string ASSType { get; set; }
        public Double PRIVal { get; set; }
        public string PRIUnit { get; set; }
        public string PRIType { get; set; }
        public decimal PRIMPro { get; set; }
        public bool CheckRow { get; set; }

        public static explicit operator ModeleOptTarAffNouvTarif(OptTarAffNouvTarifDto optTarAffNouvTarifDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<OptTarAffNouvTarifDto, ModeleOptTarAffNouvTarif>().Map(optTarAffNouvTarifDto);
        }

        public static OptTarAffNouvTarifDto LoadDto(ModeleOptTarAffNouvTarif modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleOptTarAffNouvTarif, OptTarAffNouvTarifDto>().Map(modele);
        }
    }
}