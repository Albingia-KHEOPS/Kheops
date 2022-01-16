using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Referentiel {
    public enum AlimentationValue
    {
        [BusinessCode("")] None,
        [BusinessCode("A")] Assiette,
        [BusinessCode("I")] Inventaire,
        [BusinessCode("B")] AssiettePrime,
        [BusinessCode("C")] Prime
    }
    public class Alimentation : RefValue
    {
        public const string None = "";
        public const string Assiette = "A";
        public const string Inventaire = "I";
        public const string AssiettePrime = "B";
        public const string Prime = "C";
    }
}
