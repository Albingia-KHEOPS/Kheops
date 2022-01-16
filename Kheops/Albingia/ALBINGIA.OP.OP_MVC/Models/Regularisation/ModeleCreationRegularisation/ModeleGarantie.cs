using EmitMapper;
using OP.WSAS400.DTO.Regularisation;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation
{
    public class ModeleGarantie
    {
        public Int32 IdFormule { get; set; }
        public Int32 IdOption { get; set; }
        public long IdRCFR { get; set; }
        public string CodeFormule { get; set; }
        public string LibFormule { get; set; }
        public Int32 IdGarantie { get; set; }
        public Int32 SeqGaran { get; set; }
        public string CodeGarantie { get; set; }
        public string LibGarantie { get; set; }
        public DateTime? EntreeGarantie { get; set; }
        public string EntreeGarantieStr
        {
            get
            {
                if (EntreeGarantie.HasValue)
                    return EntreeGarantie.Value.Day.ToString().PadLeft(2, '0') + "/" + EntreeGarantie.Value.Month.ToString().PadLeft(2, '0') + "/" + EntreeGarantie.Value.Year;
                else
                    return string.Empty;
            }
        }
        public DateTime? SortieGarantie { get; set; }
        public string SortieGarantieStr
        {
            get
            {
                if (SortieGarantie.HasValue)
                    return SortieGarantie.Value.Day.ToString().PadLeft(2, '0') + "/" + SortieGarantie.Value.Month.ToString().PadLeft(2, '0') + "/" + SortieGarantie.Value.Year;
                else
                    return string.Empty;
            }
        }
        public string CodeTypeRegule { get; set; }
        public string LibTypeRegule { get; set; }
        public Int32 IsGarUsed { get; set; }
        public bool IsUsed
        {
            get
            {
                return IsGarUsed > 0;
            }
        }

        public static explicit operator ModeleGarantie(RegularisationGarantieDto modelDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<RegularisationGarantieDto, ModeleGarantie>().Map(modelDto);
        }

    }
}