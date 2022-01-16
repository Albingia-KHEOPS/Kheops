using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegulGar
{
    public class PerioderegulGarantie
    {
        public int  Code { get; set; }
        public string Situation { get; set; }
        public Int32 PeriodeRegulDeb { get; set; }
        public Int32 PeriodeRegulFin { get; set; }
        public double AssietteValeur { get; set; }
        public double TauxForfaitHTValeur { get; set; }
        public string TauxForfaitHTUnite { get; set; }
        public double  MontantRegulHF { get; set; }
        public string idLot { get; set; }
        public string idregul { get; set; }
        public string idGar { get; set; }
        public string codeOpt { get; set; } 
        public string codeRsq { get; set; }
        public string codeFor { get; set; }
        public string codeGar { get; set; }
        public string libGar { get; set; }
        public long IdRCFR { get; set; }
        public string PeriodeRegulDebStr
        {
            get
            {
                DateTime? DateDebRegul = AlbConvert.ConvertIntToDate(Convert.ToInt32(PeriodeRegulDeb));
                if (DateDebRegul.HasValue)
                    return DateDebRegul.Value.Day.ToString().PadLeft(2, '0') + "/" + DateDebRegul.Value.Month.ToString().PadLeft(2, '0') + "/" + DateDebRegul.Value.Year;
                else
                    return string.Empty;
            }
        }
        public string PeriodeRegulFinStr
        {
            get
            {
                DateTime? DateFinRegul = AlbConvert.ConvertIntToDate(Convert.ToInt32(PeriodeRegulFin));
                if (DateFinRegul.HasValue)
                    return DateFinRegul.Value.Day.ToString().PadLeft(2, '0') + "/" + DateFinRegul.Value.Month.ToString().PadLeft(2, '0') + "/" + DateFinRegul.Value.Year;
                else
                    return string.Empty;
            }
        }

        public static explicit operator PerioderegulGarantie(LigneMouvtGarantieDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneMouvtGarantieDto, PerioderegulGarantie>().Map(modeleDto);
        }

    }
}