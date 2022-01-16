using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.Services.REST.wsadel {
    internal class CalculChargementRequest {
        public CalculChargementRequestData[] Donnees { get; set; }
    }

    internal class CalculChargementRequestData {
        public int SinistreAnnee { get; set; }
        public int SinistreNumero { get; set; }
        public string SinistreSousBranche { get; set; } = string.Empty;
        public int DateDebut { get; set; } = 0;
        public int DateFin { get; set; } = 99999999;
    }
}
