using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class CotisationsPlatDto
    {
        #region  CotisationInfoGarantieDto
        [DataMember]
        [Column(Name = "SUBCOTISHT")]
        public Double SubCotisationHT { get; set; }
        [DataMember]
        [Column(Name = "SUBCOTISTAXE")]
        public Double SubCotisationTaxe { get; set; }
        [DataMember]
        [Column(Name = "SUBCOTISTTC")]
        public Double SubCotisationTTC { get; set; }

        #region Catnat
        [DataMember]
        [Column(Name = "CATNAT")]
        public Double CatnatAssietteValeur { get; set; }

        [DataMember]
        public Double CatnatAssietteUnite { get; set; }
        [DataMember]
        public Double CatnatTauxValeur { get; set; }
        [DataMember]
        public Double CatnatTauxUnite { get; set; }
        [DataMember]
        [Column(Name = "COTISCATNATHT")]
        public Double CatnatCotisationHT { get; set; }
        [DataMember]
        [Column(Name = "COTISCATNATTAXE")]
        public Double CatnatCotisationTaxe { get; set; }
        [DataMember]
        [Column(Name = "COTISCATNATTTC")]
        public Double CatnatCotisationTTC { get; set; }
        #endregion

        #region Gareat
        [DataMember]
        [Column(Name = "BASEATTENTAT")]
        public Double GareatAssietteValeur { get; set; }
        [DataMember]
        public Double GareatAssietteUnite { get; set; }
        [DataMember]
        [Column(Name = "TAUXCOTISATTENTAT")]
        public Double GareatTauxValeur { get; set; }
        [DataMember]
        public Double GareatTauxUnite { get; set; }
        [DataMember]
        [Column(Name = "COTISATTENTATHT")]
        public Double GareatCotisationHT { get; set; }
        [DataMember]
        public Double GareatCotisationTaxe { get; set; }
        [DataMember]
        [Column(Name = "COTISATTENTATTTC")]
        public Double GareatCotisationTTC { get; set; }
        #endregion

        #region Comission
        [DataMember]
        [Column(Name = "TAUXCOMHORSCATNATSTD")]
        public Double CommissionTauxStd { get; set; }
        [DataMember]
        [Column(Name = "MONTANTCOMHORSCATNATSTD")]
        public Double CommissionMontantStd { get; set; }
        [DataMember]
        [Column(Name = "TAUXCOMHORSCATNATFOR")]
        public Double CommissionTauxForce { get; set; }
        [DataMember]
        [Column(Name = "MONTANTCOMHORSCATNATFOR")]
        public Double ComissionMontantForce { get; set; }
        [DataMember]
        [Column(Name = "TAUXCOMCATNATSTD")]
        public Double ComissionTauxStdCatNat { get; set; }
        [DataMember]
        [Column(Name = "MONTANTCOMCATNATSTD")]
        public Double ComissionMontantStdCatNat { get; set; }
        [DataMember]
        [Column(Name = "TAUXCOMCATNATFOR")]
        public Double ComissionTauxForceCatNat { get; set; }
        [DataMember]
        [Column(Name = "MONTANTCOMCATNATFOR")]
        public Double ComissionMontantForceCatNat { get; set; }
        #endregion

        [DataMember]
        [Column(Name = "COEFCOM")]
        public Double CoefCom { get; set; }
        [DataMember]
        [Column(Name = "TOTALHORSFRAISHT1")]
        public Double TotalHorsFraisHT1 { get; set; }
        [DataMember]
        [Column(Name = "TOTALHORSFRAISHT2")]
        public Double TotalHorsFraisHT2 { get; set; }
        [DataMember]
        [Column(Name = "TAXETOTALHORSFRAIS")]
        public Double TotalHorsFraisTaxe { get; set; }
        [DataMember]
        [Column(Name = "TOTALHORSFRAISTTC")]
        public Double TotalHorsFraisTTC { get; set; }
        [DataMember]
        [Column(Name = "FRAISHT")]
        public Double FraisHT { get; set; }
        [DataMember]
        [Column(Name = "TAXEFRAIS")]
        public Double FraisTaxe { get; set; }
        [DataMember]
        [Column(Name = "TAUXTAXEFGA")]
        public Double FGATaxe { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTAXEFGA")]
        public Single FGATTC { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTOTALHT1")]
        public Double TotalHT1 { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTOTALHT2")]
        public Double TotalHT2 { get; set; }
        [DataMember]
        [Column(Name = "TAXEMONTANTTOTAL")]
        public Double TotalTaxe { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTOTALTTC1")]
        public Double TotalTTC1 { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTOTALTTC2")]
        public Double TotalTTC2 { get; set; }
        #endregion

        [DataMember]
        [Column(Name = "COMMENTFORCE")]
        public string CommentForce { get; set; }
        [DataMember]
        [Column(Name = "MONTANTFORCE")]
        public string MontantForce { get; set; }

        [DataMember]
        [Column(Name = "TYPEPART")]
        public string TypePart { get; set; }
        [DataMember]
        [Column(Name = "TYPEPERIODE")]
        public string TypePeriode { get; set; }

        [DataMember]
        [Column(Name = "NATURECONTRAT")]
        public string NatureContrat { get; set; }



    }
}
