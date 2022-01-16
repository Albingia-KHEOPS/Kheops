using Albingia.Kheops.OP.Domain.Transverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class PrimesGareatDto {
        public PrimesGareatDto() {
            Primes = new List<(PrimeGarantieGareatDto primeComtable, PrimeGarantieGareatDto primeDevise)>();
        }
        public ICollection<(PrimeGarantieGareatDto primeComtable, PrimeGarantieGareatDto primeDevise)> Primes { get; }

        public PrimeGarantieGareatDto MontantTotal => !Primes.Any() ? new PrimeGarantieGareatDto() : new PrimeGarantieGareatDto {
            MontantTotal = Primes.Sum(x => x.primeDevise.MontantTotal),
            MontantHorsTaxe = Primes.Sum(x => x.primeDevise.MontantHorsTaxe),
            MontantTaxe = Primes.Sum(x => x.primeDevise.MontantTaxe)
        };
    }
}
