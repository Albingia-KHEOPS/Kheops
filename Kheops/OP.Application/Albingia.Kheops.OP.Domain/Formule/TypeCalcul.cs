using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public enum TypeCalcul
    {
        [BusinessCode("")] Aucune,
        [BusinessCode("X")] Multiplier,
        [BusinessCode("M")] Montant
    }
}