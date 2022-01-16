
using System;

namespace Albingia.Kheops.DTO {
    public class RetardPaiementDto {
        public AffaireDto Folder { get; set; }
        public PrimeDto Prime { get; set; }
        public DateTime DateLimit { get; set; }
    }
}
