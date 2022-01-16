using ALBINGIA.Framework.Common.Tools;
using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegulGar
{
    public class LigneGarantieRegul
    {
        // Risque
        [Display(Name = "CODERSQ")]
        public Int32 CodeRisque { get; set; }
        [Display(Name = "Risque")]
        public string RisqueIdentification { get; set; }
        [Display(Name = "CODTAXEREGIME")]
        public string CodeTaxeRegime { get; set; }
        [Display(Name = "Régime de taxe")]
        public string RegimeTaxe { get; set; }
        
        // Formule
        public string CodeGar { get; set; }

        public long IdGar { get; set; }

        [Display(Name = "CODEFOR")]
        public Int32 CodeFormule { get; set; }
        [Display(Name = "Formule")]
        public string FormuleDescriptif { get; set; }
        public string LettreFor { get; set; }
        // Garantie

        public string LibGarantie { get; set; }
        public Int64 DateDebPeriodGenerale { get; set; }
        public Int64 DateFinPeriodGenerale { get; set; }

        [Display(Name = "CodeTypeRegule")]
        public string CodeTypeRegule { get; set; }
        [Display(Name = "Type de régul")]
        public string TypeRegul { get; set; }
        [Display(Name = "Type de Taxe")]
        public String CodeTaxeGar { get; set; }
     

        public static explicit operator LigneGarantieRegul(LigneRegularisationGarantieDto modeleDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<LigneRegularisationGarantieDto, LigneGarantieRegul>().Map(modeleDto);
        }

        public string DateDebPeriodGeneraleStr
        {
            
            get
            {
                DateTime? DateDebGen = AlbConvert.ConvertIntToDate(Convert.ToInt32(DateDebPeriodGenerale));

                if (DateDebGen.HasValue)
                    return DateDebGen.Value.Day.ToString().PadLeft(2, '0') + "/" + DateDebGen.Value.Month.ToString().PadLeft(2, '0') + "/" + DateDebGen.Value.Year;
                else
                    return string.Empty;
            }
        }
        public string DateFinPeriodGeneraleStr
        {

            get
            {
                DateTime? DateFinGen = AlbConvert.ConvertIntToDate(Convert.ToInt32(DateFinPeriodGenerale));

                if (DateFinGen.HasValue)
                    return DateFinGen.Value.Day.ToString().PadLeft(2, '0') + "/" + DateFinGen.Value.Month.ToString().PadLeft(2, '0') + "/" + DateFinGen.Value.Year;
                else
                    return string.Empty;
            }
        }
    }
}