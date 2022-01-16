using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Cotisations
{
    public class QuittanceDto
    {
        [DataMember]
        [Column(Name = "CODEFORMULE")]
        public Int32 CodeFormule { get; set; }
        [DataMember]
        [Column(Name = "LETTREFORMULE")]
        public string LettreFormule { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEFORMULE")]
        public String LibelleFormule { get; set; }

        [DataMember]
        [Column(Name = "CODERISQUE")]
        public Int32 CodeRisque { get; set; }
        [DataMember]
        [Column(Name = "LIBELLERISQUE")]
        public String LibelleRisque { get; set; }

        [DataMember]
        [Column(Name = "HTHORSCATNAT")]
        public Double HtHorsCatnat { get; set; }
        [DataMember]
        [Column(Name = "CATNAT")]
        public Double CatNat { get; set; }
        [DataMember]
        [Column(Name = "TAXES")]
        public Double Taxes { get; set; }
        [DataMember]
        [Column(Name = "TTC")]
        public Double Ttc { get; set; }

        [DataMember]
        [Column(Name = "FINEFFETANNEE")]
        public Int16 FinEffetAnnee { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETMOIS")]
        public Int16 FinEffetMois { get; set; }
        [DataMember]
        [Column(Name = "FINEFFETJOUR")]
        public Int16 FinEffetJour { get; set; }

        [DataMember]
        [Column(Name = "CODEPERIODICITE")]
        public String CodePeriodicite { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEPERIODICITE")]
        public String LibellePeriodicite { get; set; }

        [DataMember]
        [Column(Name = "CODEOPERATION")]
        public Int16 CodeOperation { get; set; }
        [DataMember]
        [Column(Name = "LIBELLEOPERATION")]
        public String LibelleOperation { get; set; }

        [DataMember]
        [Column(Name = "INDICE")]
        public Double indice { get; set; }

        [DataMember]
        [Column(Name = "DEBUTPERIODEANNEE")]
        public Int16 DebutPeriodeAnnee { get; set; }
        [DataMember]
        [Column(Name = "DEBUTPERIODEMOIS")]
        public Int16 DebutPeriodeMois { get; set; }
        [DataMember]
        [Column(Name = "DEBUTPERIODEJOUR")]
        public Int16 DebutPeriodeJour { get; set; }

        [DataMember]
        [Column(Name = "FINPERIODEANNEE")]
        public Int16 FinPeriodeAnnee { get; set; }
        [DataMember]
        [Column(Name = "FINPERIODEMOIS")]
        public Int16 FinPeriodeMois { get; set; }
        [DataMember]
        [Column(Name = "FINPERIODEJOUR")]
        public Int16 FinPeriodeJour { get; set; }

        [DataMember]
        [Column(Name = "TAUXHRCATNATRETENU")]
        public Double TauxHRCatNatRetenu { get; set; }
        [DataMember]
        [Column(Name = "MONTANTCOMMISSHRCATNATRETENU")]
        public Double MontantCommissHRCatNatRetenu { get; set; }
        [DataMember]
        [Column(Name = "TAUXCATNATRETENU")]
        public Double TauxCatNatRetenu { get; set; }
        [DataMember]
        [Column(Name = "MONTANTCOMMISSRETENU")]
        public Double MontantCommissRetenu { get; set; }
        [DataMember]
        [Column(Name = "TOTALCOMMISSRETENU")]
        public Double TotalCommissRetenu { get; set; }

        [DataMember]
        [Column(Name = "TOTALHRCATNATHT")]
        public Double TotalHorsCatNatHT { get; set; }
        [DataMember]
        [Column(Name = "TOTALHRCATNATTAXE")]
        public Double TotalHorsCatNatTaxe { get; set; }
        [DataMember]
        [Column(Name = "TOTALHRCATNATTTC")]
        public Double TotalHorsCatNatTTC { get; set; }

        [DataMember]
        [Column(Name = "CATNATHT")]
        public Double CatNatHT { get; set; }
        [DataMember]
        [Column(Name = "CATNATTAXE")]
        public Double CatNatTaxe { get; set; }
        [DataMember]
        [Column(Name = "CATNATTTC")]
        public Double CatNatTTC { get; set; }

        [DataMember]
        public decimal GareatHT { get; set; }
        [DataMember]
        public decimal GareatTaxe { get; set; }
        [DataMember]
        public decimal GareatTTC { get; set; }

        [DataMember]
        [Column(Name = "TOTALHRFRAISHT")]
        public Double TotalHorsFraisHT { get; set; }
        [DataMember]
        [Column(Name = "TOTALHRFRAISTAXE")]
        public Double TotalHorsFraisTaxe { get; set; }
        [DataMember]
        [Column(Name = "TOTALHRFRAISTTC")]
        public Double TotalHorsFraisTTC { get; set; }

        [DataMember]
        [Column(Name = "FRAISHT")]
        public Single FraisHT { get; set; }
        [DataMember]
        [Column(Name = "FRAISTAXE")]
        public Single FraisTaxe { get; set; }
        [DataMember]
        [Column(Name = "FRAISTTC")]
        public Double FraisTTC { get; set; }

        [DataMember]
        [Column(Name = "FGATAXE")]
        public Single FGATaxe { get; set; }
        [DataMember]
        [Column(Name = "FGATTC")]
        public Single FGATTC { get; set; }

        [DataMember]
        [Column(Name = "MONTANTTOTALHT")]
        public Double MontantTotalHT { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTOTALTAXE")]
        public Double MontantTotalTaxe { get; set; }
        [DataMember]
        [Column(Name = "MONTANTTOTALTTC")]
        public Double MontantTotalTTC { get; set; }

        [DataMember]
        [Column(Name="PERIODICITE")]
        public string Periodicite { get; set; }
        [DataMember]
        [Column(Name="PROCHECHJOUR")]
        public Int32 ProchEchJour { get; set; }
        [DataMember]
        [Column(Name = "PROCHECHMOIS")]
        public Int32 ProchEchMois { get; set; }
        [DataMember]
        [Column(Name = "PROCHECHAN")]
        public Int32 ProchEchAn { get; set; }
        [DataMember]
        [Column(Name = "MNTHTHC")]
        public Double MntHTHC { get; set; }
        [DataMember]
        [Column(Name = "MNTC")]
        public Double MntC { get; set; }


        [DataMember]
        public string TauxStd { get; set; }
        [DataMember]
        public string TauxStdCatNat { get; set; }
    }
}
