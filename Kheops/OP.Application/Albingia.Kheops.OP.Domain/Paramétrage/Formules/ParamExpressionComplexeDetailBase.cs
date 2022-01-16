using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Referentiel;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    public abstract class ParamExpressionComplexeDetailBase
    {
        public long Id { get; set; }

        public long ExprId { get; set; }

        public int Ordre { get; set; }

        public decimal Valeur { get; set; }

        public Unite Unite { get; set; }

        public BaseDeCalcul CodeBase { get; set; }



    }
}