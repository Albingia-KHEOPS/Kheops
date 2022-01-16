using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public class PeriodeGarantie {
        public long Id { get; set; }
        public int NumeroRisque { get; set; }
        public int NumeroFormule { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal Base { get; set; }
        public decimal BaseRegul { get; set; }

        /// <summary>
        /// MB&CA Définitif ou CA 2
        /// </summary>
        public decimal Definitif2 { get; set; }

        /// <summary>
        /// MB&CA Définitif ou 2 Prime
        /// </summary>
        public decimal Definitif2Prime { get; set; }
        public decimal Definitif2Taux { get; set; }
        public string UniteTauxBase { get; set; }
        public string UniteTaux2 { get; set; }
        public decimal Assiette => UniteTaux2.IsEmptyOrNull() ? BaseRegul : Definitif2;
        public decimal TauxBase { get; set; }
        public decimal ValeurHT => UniteTaux2.IsEmptyOrNull()
            ? (UniteTauxBase == UniteBase.Devise.AsCode() ? Base : TauxBase)
            : (UniteTaux2 == UniteBase.Devise.AsCode() ? Definitif2Prime : Definitif2Taux);
        public string UniteTauxHT => UniteTaux2.IsEmptyOrNull() ? UniteTauxBase : UniteTaux2;
        public decimal MontantHTHorsCATNAT { get; set; }
        public bool IsValidated { get; set; }
        public string CodeGarantie { get; set; }
    }
}
