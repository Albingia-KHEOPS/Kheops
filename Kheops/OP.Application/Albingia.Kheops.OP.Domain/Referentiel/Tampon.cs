using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Tampon : RefValue
    {
        public enum TamponValues
        {
            [BusinessCode("O")]
            Original,
            [BusinessCode("C")]
            Copie,
            [BusinessCode("D")]
            Duplicata
        }
    }
}