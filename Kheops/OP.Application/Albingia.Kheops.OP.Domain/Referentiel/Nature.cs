using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Referentiel
{

    public enum NatureValue
    {
        [BusinessCode("")] None,
        [BusinessCode("C")] Comprise,
        [BusinessCode("E")] Exclue,
        [BusinessCode("A")] Accordee,
    }

    public enum NatureAffaireCode
    {
        [BusinessCode("")] CentPourCent,
        [BusinessCode("A")] Aperition,
        [BusinessCode("C")] Coassurance,
        [BusinessCode("D")] CoassuranceBourse,
        [BusinessCode("E")] AperitionBourse,
    }

    public class NatureAffaire : RefValue
    {
    }
}