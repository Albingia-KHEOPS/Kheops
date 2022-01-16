using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain {
    public enum ModeTaxation {
        [BusinessCode("HT")]
        HorsTaxes,

        [BusinessCode("TTC")]
        ToutesTaxesComprises
    }
}
