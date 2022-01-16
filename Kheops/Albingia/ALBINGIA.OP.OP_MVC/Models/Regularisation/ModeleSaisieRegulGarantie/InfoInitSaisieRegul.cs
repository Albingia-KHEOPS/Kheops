using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleSaisieRegulGarantie
{
    public class InfoInitSaisieRegul
    {
        public Int32 Motif_Inf { get; set; }

        public string TitreGar_STD { get; set; }

        public Int32 DateDeb_STD { get; set; }

        public Int32 DateFin_STD { get; set; }

        public Int32 CodeRsq_STD { get; set; }

        public string LibRsq_STD { get; set; }

        public string CodeRgt_STD { get; set; }

        public string LibRgt_STD { get; set; }

        public Int32 CodeFor_STD { get; set; }

        public string LibFor_STD { get; set; }

        public string LettreFor_STD { get; set; }
        public string LibTaxe_STD { get; set; }
        public string CodeTaxe_STD { get; set; }
        public string TypeGrille { get; set; }

        public string TypeReguleGar_STD { get; set; }

        public string LibReguleGar_STD { get; set; }

        public string GarAuto_STD { get; set; }

        public Int32 ValGarAuto_STD { get; set; }

        public string UnitGarAuto_STD { get; set; }

        public double TxAppel_STD { get; set; }

        public Single TxAttentat_STD { get; set; }

        public double MntCotisProv_STD { get; set; }

        public double MntCotisAquis_STD { get; set; }

        public double PrevAssiette_STD { get; set; }

        public double PrevTaux_STD { get; set; }

        public string PrevUnite_STD { get; set; }

        public string PrevCodTaxe_STD { get; set; }

        public double PrevMntHt_STD { get; set; }

        public double PrevTax_STD { get; set; }

        public double DefAssiette_STD { get; set; }

        public double DefTaux_STD { get; set; }

        public string DefUnite_STD { get; set; }

        public string DefCodTaxe_STD { get; set; }

        public double DefVmntHt_STD { get; set; }

        public double DefVtax_STD { get; set; }

        public double MntCotisEmis_STD { get; set; }

        public double MntTxEmis_STD { get; set; }

        public double MntForceEmis_STD { get; set; }

        public double MntForceTx_STD { get; set; }

        public double Coef_STD { get; set; }

        public double MntRegulHt_STD { get; set; }

        public double Attentat_STD { get; set; }

        public double MntForceCalc_STD { get; set; }

        public string Force0_STD { get; set; }

        public Int32 NbYearRsq_PB { get; set; }

        public double TxRistRsq_PB { get; set; }

        public double TSeuilSp_PB { get; set; }

        public double TxCotisRetRsq_PB { get; set; }

        public decimal RistAnticipee_PB { get; set; }

        public double CotisEmise_PB { get; set; }

        public double TxAppelPbns_PB { get; set; }

        public double CotisDue_PB { get; set; }

        public Int32 NbYearRegul_PB { get; set; }

        public double CotisRetenue_PB { get; set; }

        public double TxCotisRet_PB { get; set; }

        public double ChargeSin_PB { get; set; }

        public double Pbns_PB { get; set; }
        //public Int32 CotisAnticipee_PB { get; set; }
        public double TxRistRegul_PB { get; set; }
        public double RistAnticipeeReguL_PB { get; set; }
        public Int32 DateDebRegul { get; set; }
        public Int32 DateFinRegul { get; set; }
        public string topFrc { get; set; }
        public string PrimePro { get; set; }
        public string motif { get; set; }
        public string ErrorStr { get; set; }
        public bool SuivantDesactiv { get; set; }
        public bool relaod { get; set; }
        public static explicit operator InfoInitSaisieRegul(SaisieGarInfoDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<SaisieGarInfoDto, InfoInitSaisieRegul>().Map(modeleDto);
        }

        public static SaisieGarInfoDto LoadDto(InfoInitSaisieRegul modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<InfoInitSaisieRegul, SaisieGarInfoDto>().Map(modele);
        }

        public string DateDeb_STD_STR
        {
            get
            {
                DateTime? DateDebPeriode = AlbConvert.ConvertIntToDate(DateDeb_STD);
                if (DateDebPeriode.HasValue)
                    return DateDebPeriode.Value.Day.ToString().PadLeft(2, '0') + "/" + DateDebPeriode.Value.Month.ToString().PadLeft(2, '0') + "/" + DateDebPeriode.Value.Year;
                else
                    return string.Empty;
            }
        }
        public string DateFin_STD_STR
        {
            get
            {
                DateTime? DateDebPeriode = AlbConvert.ConvertIntToDate(DateFin_STD);
                if (DateDebPeriode.HasValue)
                    return DateDebPeriode.Value.Day.ToString().PadLeft(2, '0') + "/" + DateDebPeriode.Value.Month.ToString().PadLeft(2, '0') + "/" + DateDebPeriode.Value.Year;
                else
                    return string.Empty;
            }
        }


        public string DateDebRegul_STR
        {
            get
            {
                DateTime? DateDebRegule = AlbConvert.ConvertIntToDate(DateDebRegul);
                if (DateDebRegule.HasValue)
                    return DateDebRegule.Value.Day.ToString().PadLeft(2, '0') + "/" + DateDebRegule.Value.Month.ToString().PadLeft(2, '0') + "/" + DateDebRegule.Value.Year;
                else
                    return string.Empty;
            }
        }
        public string DateFinRegul_STR
        {
            get
            {
                DateTime? DateFinRegule = AlbConvert.ConvertIntToDate(DateFinRegul);
                if (DateFinRegule.HasValue)
                    return DateFinRegule.Value.Day.ToString().PadLeft(2, '0') + "/" + DateFinRegule.Value.Month.ToString().PadLeft(2, '0') + "/" + DateFinRegule.Value.Year;
                else
                    return string.Empty;
            }
        }
    }
}