using Albingia.Kheops.OP.Domain.Referentiel;
using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Garantie {
    public class GareatState {
        public decimal? LCIGenerale { get; set; }
        public decimal? PrimeGaranties { get; set; }
        public TrancheGareat TrancheGareat { get; set; }
        public decimal Prime { get; set; }
        public RegimeTaxe RegimeTaxe { get; set; }

        public void Compute() {
            var tranche = (TrancheGareat.TrancheLCI(LCIGenerale) ?? TrancheGareat.TranchePrime(PrimeGaranties)) ?? new TrancheGareat();
            tranche.CopyTo(TrancheGareat);
            if (TrancheGareat.ActualRate == decimal.Zero || (RegimeTaxe?.Code).IsIn(RegimeTaxe.Monaco, RegimeTaxe.MonacoProfessionLiberaleHabitation)) {
                Prime = 0;
            }
            else {
                Prime = Math.Round(TrancheGareat.ActualRate, 4) * PrimeGaranties.GetValueOrDefault();
            }
        }
    }
}
