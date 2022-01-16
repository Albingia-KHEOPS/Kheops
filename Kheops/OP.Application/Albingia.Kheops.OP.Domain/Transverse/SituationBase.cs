using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public enum SituationBase {
        [BusinessCode("V")]
        Validee = 1,
        [BusinessCode("N")]
        NonValidee
    }
}
