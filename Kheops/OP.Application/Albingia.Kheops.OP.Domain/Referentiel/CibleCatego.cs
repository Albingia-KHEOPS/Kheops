namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class CibleCatego
    {
        /// <summary>
        /// Cible
        /// </summary>
        virtual public Cible Cible { get; set; }
        /// <summary>
        /// Branche
        /// </summary>
        virtual public Branche Branche { get; set; }
        /// <summary>
        /// Categorie
        /// </summary>
        virtual public Categorie Categorie { get; set; }
        public string SousBranche { get; set; }
        public long Id { get; set; }

        public override int GetHashCode()
        {
            return ((((Cible.Code.GetHashCode() * 23 + Branche.Code.GetHashCode()) * 23 + Categorie.CategoryCode.GetHashCode()) * 23) + SousBranche.GetHashCode()) + 17;
        }
        public override bool Equals(object obj)
        {
                return (obj is CibleCatego x) && x.Branche.Code == this.Branche.Code
                    && x.Cible.Code == this.Cible.Code
                    && x.Categorie.CategoryCode == this.Categorie.CategoryCode
                    && x.SousBranche == this.SousBranche;
        }
    }
}