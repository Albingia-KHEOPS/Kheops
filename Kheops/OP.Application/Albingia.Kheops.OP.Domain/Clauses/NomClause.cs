namespace Albingia.Kheops.OP.Domain.Clauses
{
    public class NomClause
    {
        public NomClause(string nom1, string nom2, int nom3, int version=0)
        {
            this.Nom1 = nom1;
            this.Nom2 = nom2;
            this.Nom3 = nom3;
            this.NumeroVersion = version;
        }
        /// <summary>
        /// Nom1 / Categorie
        /// </summary>
        public string Nom1 { get; set; }

        /// <summary>
        /// Nom2 / Section
        /// </summary>
        public string Nom2 { get; set; }

        /// <summary>
        /// Nom3 / Numero 
        /// </summary>
        public int Nom3 { get; set; }

        /// <summary>
        /// NumeroVersion
        /// </summary>
        public int NumeroVersion { get; set; }

        public string ComposedName => $"{Nom1}_{Nom2}_{Nom3}";
    }
}