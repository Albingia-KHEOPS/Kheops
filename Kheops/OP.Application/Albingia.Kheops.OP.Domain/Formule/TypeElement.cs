using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public enum TypeElement {
        [BusinessCode("O")] Option = 1,
        [BusinessCode("V")] Volet,
        [BusinessCode("B")] Bloc,
        [BusinessCode("M")] Modele,
        [BusinessCode("G")] Garantie
    }
}