using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Model
{
    public enum CaractereSelection
    {
        [BusinessCode("O")]
        Obligatoire,
        [BusinessCode("P")]
        Propose,
        [BusinessCode("F")]
        Facultatif,
        [BusinessCode("S")]
        Suggere,
        [BusinessCode("B")]
        DeBase
    }
}
