using System;

namespace ALBINGIA.OP.OP_MVC.Common {
    [Flags]
    public enum FlowAccessMode {
        ReadOnly,
        ReadWrite,
        ModifHorsAvenant,
        Engagement
    }
}