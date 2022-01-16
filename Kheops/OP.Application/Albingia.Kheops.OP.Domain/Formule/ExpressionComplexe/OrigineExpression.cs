using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe {
    public enum OrigineExpression
    {
        [BusinessCode("")]
        None,
        [BusinessCode("R")]
        Referentiel = 1,
        [BusinessCode("S")]
        Saisie = 2
    }

}