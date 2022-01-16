
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Affaire
{
    public enum EtatAffaire
    {
        [BusinessCode("")] Inconnu = 0,
        [BusinessCode("A")] NonValidee,
        [BusinessCode("N")] NonValidable,
        [BusinessCode("R")] Realisee,
        [BusinessCode("V")] Validee
    }
}