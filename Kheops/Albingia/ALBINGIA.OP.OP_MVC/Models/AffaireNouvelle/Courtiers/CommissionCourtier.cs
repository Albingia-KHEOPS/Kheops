using EmitMapper;
using OP.WSAS400.DTO.AffaireNouvelle;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.Courtiers
{
    public class CommissionCourtier
    {
        public double TauxStandardHCAT { get; set; }
        public double TauxStandardCAT { get; set; }
        public double TauxContratHCAT { get; set; }
        public double TauxContratCAT { get; set; }
        [Display(Name = "Commentaires")]
        public string Commentaires { get; set; }
        public string Erreur { get; set; }
        [Display(Name = "Taux hors CATNAT*")]
        public string IsStandardHCAT { get; set; }
        [Display(Name = "Taux CATNAT*")]
        public string IsStandardCAT { get; set; }

        public bool IsModeAvenant { get; set; }
        public bool IsAvenantModificationLocale { get; set; }
        public bool IsTraceAvnExist { get; set; }
        public DateTime? DateEffetAvenantModificationLocale { get; set; }
        public double CommissionAperition { get; set; }

        public static explicit operator CommissionCourtier(CommissionCourtierDto data)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<CommissionCourtierDto, CommissionCourtier>().Map(data);
        }
    }
}