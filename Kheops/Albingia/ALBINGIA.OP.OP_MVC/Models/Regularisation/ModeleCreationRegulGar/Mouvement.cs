using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegulGar
{
    public class Mouvement
    {
        public Int64 MouvementPeriodeDeb { get; set; }
        public Int64 MouvementPeriodeFin { get; set; }
        public double AssietteValeurMvt { get; set; }
        public double Taux { get; set; }
        public string Unite { get; set; }

        public string MouvementPeriodeDebStr
        {
            get
            {
                DateTime? DateDebPeriode = AlbConvert.ConvertIntToDate(Convert.ToInt32(MouvementPeriodeDeb));
                if (DateDebPeriode.HasValue)
                    return DateDebPeriode.Value.Day.ToString().PadLeft(2, '0') + "/" + DateDebPeriode.Value.Month.ToString().PadLeft(2, '0') + "/" + DateDebPeriode.Value.Year;
                else
                    return string.Empty;
            }
        }

        public string MouvementPeriodeFinStr
        {
            get
            {
                DateTime? DateFinPeriode = AlbConvert.ConvertIntToDate(Convert.ToInt32(MouvementPeriodeFin));
                if (DateFinPeriode.HasValue)
                    return DateFinPeriode.Value.Day.ToString().PadLeft(2, '0') + "/" + DateFinPeriode.Value.Month.ToString().PadLeft(2, '0') + "/" + DateFinPeriode.Value.Year;
                else
                    return string.Empty;
            }
        }

        public static explicit operator Mouvement(LigneMouvementDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneMouvementDto, Mouvement>().Map(modeleDto);
        }
    }
}