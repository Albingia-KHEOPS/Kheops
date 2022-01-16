using System;
using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Formule {
    public class PorteeGarantie
    {
        public long Id { get; set; }
        public long GarantieId { get; set; }
        public int RisqueId { get; set; }
        public int NumObjet { get; set; }
        public virtual ValeursUnite ValeursPrime {get; set;}
        public decimal Montant { get; set; }
        public TypeCalcul TypeCalcul { get; set; }
        public string CodeAction { get; set; }
        public ActionValue Action { get; set; }

        internal void ResetIds() {
            Id = default;
            GarantieId = default;
        }
    }

    public enum ActionValue {
        [BusinessCode("A")] Accorde,
        [BusinessCode("E")] Exclu,
    }
}
