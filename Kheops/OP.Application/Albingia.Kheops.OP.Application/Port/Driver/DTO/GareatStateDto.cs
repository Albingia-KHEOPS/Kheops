using Albingia.Kheops.OP.Domain.Garantie;

namespace Albingia.Kheops.OP.DTO {
    public struct GareatStateDto {
        public decimal? LCIGenerale;
        public decimal? PrimeGaranties;
        public string CodeTranche;
        public decimal TauxTranche;
        public decimal TauxFraisGeneraux;
        public decimal TauxCommissions;
        public decimal TauxRetenu;
        public decimal Prime;
        public string CodeRegimeTaxe;
    }
}
