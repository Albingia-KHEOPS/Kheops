namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Adresse
    {
        public string Ligne1 { get; set; }
        public string Ligne2 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public Pays Pays { get; set; }
    }
}