using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public enum SituationDocumentLot
    {
        [BusinessCode("")] None,
        [BusinessCode("O")] O,
        [BusinessCode("N")] N,
        [BusinessCode("E")] Traite,
        [BusinessCode("Z")] Erreur
    }
}