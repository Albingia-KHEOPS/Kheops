using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public enum SituationClause
    {
        [BusinessCode("")] None,
        [BusinessCode("A")] A,
        [BusinessCode("V")] Valide,
        [BusinessCode("Z")] Erreur
    }
}