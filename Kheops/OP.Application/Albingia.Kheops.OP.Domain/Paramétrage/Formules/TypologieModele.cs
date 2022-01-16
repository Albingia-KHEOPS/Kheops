using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    public enum TypologieModele
    {
        [BusinessCode("COA")] Coassurance,
        [BusinessCode("ITC")] ITC, 
        [BusinessCode("STD")] Standard
    }
}