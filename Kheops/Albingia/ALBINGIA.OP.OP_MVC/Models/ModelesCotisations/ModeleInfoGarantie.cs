using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCotisations
{
    [Serializable]
    public class ModeleInfoGarantie
    {
        public List<ModeleGarantie> Garanties { get; set; }
        public List<ModeleGarantie> GarantiesFS { get; set; }
        public string SubCotisationHT { get; set; }
        public string SubCotisationTaxe { get; set; }
        public string SubCotisationTTC { get; set; }
        public ModeleCatnatGareat Catnat { get; set; }
        public ModeleCatnatGareat Gareat { get; set; }
        public ModeleCommission Commission { get; set; }
        [Display(Name = "Coef comm. (%)")]
        public string CoefCom { get; set; }
        [Display(Name = "Total hors frais")]
        public decimal TotalHorsFraisHT { get; set; }
        public decimal TotalHorsFraisTaxe { get; set; }
        public decimal TotalHorsFraisTTC { get; set; }
        public decimal FraisHT { get; set; }
        public decimal FraisTaxe { get; set; }
        public decimal FraisTTC { get { return FraisHT + FraisTaxe; } }
        public decimal FGATaxe { get; set; }
        public string FGATTC { get; set; }
        public decimal TotalHT { get; set; }
        public string TotalTaxe { get; set; }
        public string TotalTTC { get { return (TotalHT + Convert.ToDecimal(TotalTaxe)).ToString(); } }
        //public bool FullScreen { get; set; }
        public bool IsReadonly { get; set; }
        public string TypePart { get; set; }
        public string TypePeriode { get; set; }
        public string NatureContrat { get; set; }
        public ModeleInfoGarantie()
        {
            this.Garanties = new List<ModeleGarantie>();
            this.Catnat = new ModeleCatnatGareat();
            this.Gareat = new ModeleCatnatGareat();
            this.Commission = new ModeleCommission();
        }

        public static explicit operator ModeleInfoGarantie(CotisationInfoGarantieDto InfoGarantieDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CotisationInfoGarantieDto, ModeleInfoGarantie>().Map(InfoGarantieDto);
        }

        public static CotisationInfoGarantieDto LoadDto(ModeleInfoGarantie modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleInfoGarantie, CotisationInfoGarantieDto>().Map(modele);
        }
    }
}