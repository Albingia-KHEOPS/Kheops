using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public enum PerimetreSelectionRegul {
        [BusinessCode("RQ")]
        Risque = 1,

        [BusinessCode("OB")]
        Objet,

        [BusinessCode("FO")]
        Formule,

        [BusinessCode("GA")]
        Garantie
    }
}
