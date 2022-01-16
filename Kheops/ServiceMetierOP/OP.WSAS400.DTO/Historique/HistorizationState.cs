using ALBINGIA.Framework.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO {
    public class HistorizationState {
        public HistorizationSteps Steps { get; set; }
        public int NumeroAvenant { get; set; }
        public char EtatAffaire { get; set; }
        public char SituationAffaire { get; set; }
        public int LienAdresse { get; set; }
        public int NewCodeAdresse { get; set; }
        public DateTime? DateSituation { get; set; }
        public DateTime? DateRemiseVigueur { get; set; }
        public DateTime? DateFinEffet { get; set; }
        public DateTime? DateResiliation { get; set; }
        public DateTime? DateEffet { get; set; }
        public string Periodicite { get; set; }
        public DateTime? EcheancePrincipale { get; set; }
        public string MotifResiliation { get; set; }
        public bool ShouldAdjustResilations {
            get {
                return (Steps & HistorizationSteps.CreateOrUpdateAvenant) == HistorizationSteps.CreateOrUpdateAvenant
                    && DateFinEffet.HasValue && DateRemiseVigueur.HasValue
                    && DateRemiseVigueur > DateFinEffet.Value.AddMinutes(1);
            }
        }
    }
}
