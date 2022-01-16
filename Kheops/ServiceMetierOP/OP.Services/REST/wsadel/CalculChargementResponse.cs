using Albingia.Kheops.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.Services.REST.wsadel {
    internal class CalculChargementResponse {
        public CALCUL_CHARGEMENTReturn CALCUL_CHARGEMENTReturn { get; set; }
    }

    internal class CALCUL_CHARGEMENTReturn {
        public CALCUL_CHARGEMENTReturnResultats ResultatChgts { get; set; }
        public string MessageRetourChgt { get; set; }
        public int P_NB2_HTTP_STATUS { get; set; }
    }

    internal class CALCUL_CHARGEMENTReturnResultats {
        public CalculChargementDto ResultatChgt { get; set; }
    }
}
