using EmitMapper;
using OP.WSAS400.DTO.FormuleGarantie;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    public class ModeleInfoDetailsGarantie
    {
        public string CodeGarantie { get; set; }
        public string LibGarantie { get; set; }
        public string CodeTypeControle { get; set; }
        public string LibTypeControle { get; set; }
        public int GroupeAlternatif { get; set; }
        public string TypeApplication { get; set; }

        public Double FranchiseValeur { get; set; }
        public string FranchiseUnite { get; set; }
        public string FranchiseType { get; set; }
        public string FranchiseMAJ { get; set; }
        public string FranchiseOblig { get; set; }

        public Double LCIValeur { get; set; }
        public string LCIUnite { get; set; }
        public string LCIType { get; set; }
        public string LCIMAJ { get; set; }
        public string LCIOblig { get; set; }

        public Double AssietteValeur { get; set; }
        public string AssietteUnite { get; set; }
        public string AssietteType { get; set; }
        public string AssietteMAJ { get; set; }
        public string AssietteOblig { get; set; }

        public Double PrimeValeur { get; set; }
        public string PrimeUnite { get; set; }
        public string PrimeType { get; set; }
        public string PrimeMAJ { get; set; }
        public string PrimeOblig { get; set; }

        public static explicit operator ModeleInfoDetailsGarantie(GarantieDetailInfoDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<GarantieDetailInfoDto, ModeleInfoDetailsGarantie>().Map(modeleDto);
        }

        public static GarantieDetailInfoDto LoadDto(ModeleInfoDetailsGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleInfoDetailsGarantie, GarantieDetailInfoDto>().Map(modele);
        }

    }
}