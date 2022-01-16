using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Garantie {
    /// <summary>
    /// <![CDATA[
    /// Tranche =  
    /// Selon LCI :
    /// Tranche « A » : Taux = 6% si : 6 000 000€ < LCI < 19 999 999€ 
    /// Tranche « B » : Taux = 12% si : 20 000 000€ < LCI < 49 999 999€
    /// Tranche « C » : Taux = 18% si : LCI> 50 000 000€
    /// Selon Prime (s’il n’y a pas de LCI) :
    /// Tranche « A » : Taux = 6% si : 6 000€ < Prime < 11 999€
    /// Tranche « B » : Taux = 12% si : 12 000€ < Prime < 49 999€
    /// Tranche « C » : Taux 18% si : Prime > 50 000€
    /// Frais généraux = information renseignée manuellement
    /// Commission = informations renseignée manuellement
    /// Taux retenu = ((Taux de la tranche / (100%-Frais généraux)) / (100%-Commission)
    /// ]]>
    /// </summary>
    public class TrancheGareat : RefValue {
        public static readonly TrancheGareat A = new TrancheGareat { Code = nameof(A), Libelle = nameof(A), Rate = 0.06M };
        public static readonly TrancheGareat B = new TrancheGareat { Code = nameof(B), Libelle = nameof(B), Rate = 0.12M };
        public static readonly TrancheGareat C = new TrancheGareat { Code = nameof(C), Libelle = nameof(C), Rate = 0.18M };
        public decimal Rate { get; set; }
        public decimal RateFraisGeneraux { get; set; }
        public decimal RateCommissions { get; set; }
        public decimal ActualRate => Rate / (1 - RateFraisGeneraux) / (1 - RateCommissions);
        public static TrancheGareat TrancheLCI(decimal? lci) {
            return lci.HasValue ? (lci >= 6000000M && lci <= 19999999M ? A.MemberwiseClone() as TrancheGareat :
                lci >= 20000000M && lci <= 49999999M ? B.MemberwiseClone() as TrancheGareat :
                lci >= 50000000M ? C.MemberwiseClone() as TrancheGareat : new TrancheGareat()) : null;
        }
        public static TrancheGareat TranchePrime(decimal? prime) {
            return prime.HasValue ? (prime >= 6000M && prime <= 11999M ? A.MemberwiseClone() :
                prime >= 12000M && prime <= 49999M ? B.MemberwiseClone() :
                prime >= 50000M ? C.MemberwiseClone() : null) as TrancheGareat : null;
        }

        /// <summary>
        /// Copies only ref properties into the target
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(TrancheGareat target) {
            base.CopyTo(target);
            target.Rate = Rate;
        }
    }
}
