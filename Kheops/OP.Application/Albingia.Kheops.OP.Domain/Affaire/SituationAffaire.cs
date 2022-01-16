
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Affaire
{
    public enum SituationAffaire
    {
        [BusinessCode("")] Inconnu = 0,
        [BusinessCode("A")] EnCours,
        [BusinessCode("W")] SansSuite,
        [BusinessCode("X")] Resiliee,
        [BusinessCode("N")] Annulee,
    }
}