using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCotisations
{
    [Serializable]
    public class ModeleCatnatGareat
    {
        public string AssietteValeur { get; set; }
        public string AssietteUnite { get; set; }
        public string TauxValeur { get; set; }
        public string TauxUnite { get; set; }
        public string CotisationHT { get; set; }
        public string CotisationTaxe { get; set; }
        public string CotisationTTC { get; set; }

        public static explicit operator ModeleCatnatGareat(CotisationCanatGareatDto CanatGareatDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CotisationCanatGareatDto, ModeleCatnatGareat>().Map(CanatGareatDto);
        }

        public static CotisationCanatGareatDto LoadDto(ModeleCatnatGareat modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleCatnatGareat, CotisationCanatGareatDto>().Map(modele);
        }
    }
}