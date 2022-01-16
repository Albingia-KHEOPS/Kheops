using Albingia.Kheops.OP.Domain.Formule;

namespace Albingia.Kheops.DTO
{
    public class TarifGarantieDto
    {
        public short NumeroTarif { get; set; }

        public ValeursOptionsTarif LCI;
        public ValeursOptionsTarif Franchise;
        public ValeursTarif FranchiseMini;
        public ValeursTarif FranchiseMax;
        public ValeursOptionsTarif PrimeValeur;

        /* KDGMNTBASE */
        public decimal PrimeMontantBase { get; set; }
        /* KDGPRIMPRO */
        public decimal PrimeProvisionnelle { get; set; }
        /* KDGTMC */
        public decimal TotalMontantCalcule { get; set; }
        /* KDGTFF */
        public decimal TotalMontantForce { get; set; }

        /* KDGCMC */
        public decimal ComptantMontantcalcule { get; set; }
        /* KDGCHT */
        public decimal ComptantMontantForceHT { get; set; }
        /* KDGCTT */
        public decimal ComptantMontantForceTTC { get; set; }
        public long Id { get; set; }
        public int? NumeroAvenant { get; set; }
    }
}