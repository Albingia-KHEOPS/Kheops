using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Document
{
    public enum TypeDocument
    {
        [BusinessCode("")]None,
        [BusinessCode("LETTYP")]LettreType,
        [BusinessCode("CP")]ConsitionsParticulieres,
        [BusinessCode("CS")]ConditionsSpecifiques,
        [BusinessCode("AI")]AI,
    }
}