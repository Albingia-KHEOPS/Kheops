using EmitMapper;
using OP.WSAS400.DTO.Cotisations;
using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCotisations
{
    [Serializable]
    public class ModeleCommission
    {
        public string TauxStd { get; set; }
        public decimal MontantStd { get; set; }
        public string TauxForce { get; set; }
        public decimal MontantForce { get; set; }
        public string TauxStdCatNat { get; set; }
        public decimal MontantStdCatNat { get; set; }
        public string TauxForceCatNat { get; set; }
        public decimal MontantForceCatNat { get; set; }
        public decimal MontantStdTotal { get { return MontantStd + MontantStdCatNat; } }
        public decimal MontantForceTotal { get { return MontantForce + MontantForceCatNat; } }
        public bool isReadonly { get; set; }
        public bool TraceCC { get; set; }

        public static explicit operator ModeleCommission(CotisationCommissionDto CommissionDto)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CotisationCommissionDto, ModeleCommission>().Map(CommissionDto);
        }

        public static CotisationCommissionDto LoadDto(ModeleCommission modele)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<ModeleCommission, CotisationCommissionDto>().Map(modele);
        }

    }
}