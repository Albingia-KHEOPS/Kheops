namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Categorie
    {
        public string BrancheCode { get; set; }
        public string SousBrancheCode { get; set; }
        public string CategoryCode { get; set; }
        public int MontantFraisAccessoires { get; set; }
        public bool ATaxeAttentant { get; set; }
        public decimal TauxComCatastropheNaturelle { get; set; }
        public bool AIndexation { get; set; }
        public string CodeIndice { get; set; }
        public bool AIndexationCapitaux { get; set; }
        public bool AIndexatioFranchise { get; set; }
        public bool AIndexationLci { get; set; }
        public bool AIndexationPrime { get; set; }
        public decimal TauxPrimeCatastropheNaturelle { get; set; }
    }
}