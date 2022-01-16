using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public enum Motif {
        [BusinessCode("M3")]
        Regularisation,
        [BusinessCode("INFERIEURE")]
        PrimeInferieure,
        [BusinessCode("CADRE")]
        PoliceCadre,
        [BusinessCode("COASS")]
        Coassurance,
        [BusinessCode("MAJO")]
        PrimeMajoree
    }
}
