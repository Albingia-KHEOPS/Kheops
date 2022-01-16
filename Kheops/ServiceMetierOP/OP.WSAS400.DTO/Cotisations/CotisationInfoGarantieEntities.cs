using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.Mapping;

namespace OP.WSAS400.DTO.Cotisations
{
    public class CotisationInfoGarantieEntities
    {
        [Column(Name = "SUBCOTISHT")]
        public double SubCotisationHT { get; set; }
        [Column(Name = "SUBCOTISTAXE")]
        public double SubCotisationTaxe { get; set; }
        [Column(Name = "SUBCOTISTTC")]
        public double SubCotisationTTC { get; set; }

        [Column(Name = "CATNAT")]
        public double CatNatAssietteValeur { get; set; }
        [Column(Name = "COTISCATNATHT")]
        public double CatNatCotisationHT { get; set; }
        [Column(Name = "COTISCATNATTAXE")]
        public double CatNatCotisationTaxe { get; set; }
        [Column(Name = "COTISCATNATTTC")]
        public double CatNatCotisationTTC { get; set; }

        [Column(Name = "BASEATTENTAT")]
        public double GareatAssietteValeur { get; set; }
        [Column(Name = "TAUXCOTISATTENTAT")]
        public double GareatTauxValeur { get; set; }
        [Column(Name = "COTISATTENTATHT")]
        public double GareatCotisationHT { get; set; }
        [Column(Name = "COTISATTENTATTAXE")]
        public double GareatCotisationTaxe { get; set; }
        [Column(Name = "COTISATTENTATTTC")]
        public double GareatCotisationTTC { get; set; }

        [Column(Name = "TAUXCOMHORSCATNATSTD")]
        public double TauxStd { get; set; }
        [Column(Name = "MONTANTCOMHORSCATNATSTD")]
        public double MontantStd { get; set; }
        [Column(Name = "TAUXCOMHORSCATNATFOR")]
        public double TauxForce { get; set; }
        [Column(Name = "MONTANTCOMHORSCATNATFOR")]
        public double MontantForce { get; set; }
        [Column(Name = "TAUXCOMCATNATSTD")]
        public double TauxStdCatNat { get; set; }
        [Column(Name = "MONTANTCOMCATNATSTD")]
        public double MontantStdCatNat { get; set; }
        [Column(Name = "TAUXCOMCATNATFOR")]
        public double TauxForceCatNat { get; set; }
        [Column(Name = "MONTANTCOMCATNATFOR")]
        public double MontantForceCatNat { get; set; }

        [Column(Name = "COEFCOM")]
        public double CoefCom { get; set; }
        [Column(Name = "TOTALHORSFRAISHT1")]
        public double TotalHorsFraisHT1 { get; set; }
        [Column(Name = "TOTALHORSFRAISHT2")]
        public double TotalHorsFraisHT2 { get; set; }
        [Column(Name = "TAXETOTALHORSFRAIS")]
        public double TotalHorsFraisTaxe { get; set; }
        [Column(Name = "TOTALHORSFRAISTTC")]
        public double TotalHorsFraisTTC { get; set; }
        [Column(Name = "FRAISHT")]
        public double FraisHT { get; set; }
        [Column(Name = "TAXEFRAIS")]
        public double FraisTaxe { get; set; }
        [Column(Name = "TAUXTAXEFGA")]
        public double FGATaxe { get; set; }
        [Column(Name = "MONTANTTAXEFGA")]
        public double FGATTC { get; set; }
        [Column(Name = "MONTANTTOTALHT1")]
        public double TotalHT1 { get; set; }
        [Column(Name = "MONTANTTOTALHT2")]
        public double TotalHT2 { get; set; }
        [Column(Name = "TAXEMONTANTTOTAL")]
        public double TotalTaxe { get; set; }
        [Column(Name = "MONTANTTOTALTTC1")]
        public double TotalTTC1 { get; set; }
        [Column(Name = "MONTANTTOTALTTC2")]
        public double TotalTTC2 { get; set; }


    }
}
