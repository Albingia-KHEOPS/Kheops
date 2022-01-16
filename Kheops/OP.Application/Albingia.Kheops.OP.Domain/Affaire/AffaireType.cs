using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Affaire
{
    public enum AffaireType
    {   
        [BusinessCode("")]
        None,
        [BusinessCode("P")]
        Contrat = 1,
        [BusinessCode("O")]
        Offre = 2
    }

}