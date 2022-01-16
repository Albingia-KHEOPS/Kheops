using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Referentiel;

namespace Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe
{
    public class ExpressionComplexeDetailLCI : ExpressionComplexeDetailBase
    {

        public decimal ConcurrentValeur { get; set; }

        public Unite ConcurrentUnite { get; set; }

        public BaseLCI ConcurrentCodeBase { get; set; }

    }
}