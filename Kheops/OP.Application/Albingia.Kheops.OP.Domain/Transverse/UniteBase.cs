using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain {
    public enum UniteBase {
        [BusinessCode("D")]
        Devise = 1,
        [BusinessCode("%")]
        PourCent,
        [BusinessCode("%0")]
        PourMille
    }
}
