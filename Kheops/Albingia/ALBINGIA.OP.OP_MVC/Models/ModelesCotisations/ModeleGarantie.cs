using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCotisations
{
    [Serializable]
    public class ModeleGarantie
    {
        public string CodeGarantie { get; set; }
        public string NomGarantie { get; set; }
        public string CodeRisque { get; set; }
        public string CodeFormule { get; set; }
        public string Tarif { get; set; }
        public string LCIValeur { get; set; }
        public string LCIUnite { get; set; }
        public string AssietteValeur { get; set; }
        public string AssietteUnite { get; set; }
        public string TauxValeur { get; set; }
        public string TauxUnite { get; set; }
        public string CotisationHT { get; set; }
        public string CotisationTaxe { get; set; }
        public string CotisationTTC { get; set; }
        public string LienKpgaran { get; set; }

        public static explicit operator ModeleGarantie(CotisationGarantieDto GarantieDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CotisationGarantieDto, ModeleGarantie>().Map(GarantieDto);
        }

        public static CotisationGarantieDto LoadDto(ModeleGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleGarantie, CotisationGarantieDto>().Map(modele);
        }

    }
}