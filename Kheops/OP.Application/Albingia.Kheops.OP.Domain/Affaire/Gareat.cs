using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire {
    public class Gareat {
        public bool IsApplique { get; set; }
        public string Tranche { get; set; }
        public string NatureCritere { get; set; }
        public decimal MontantCritere { get; set; }
        public TauxGareat TauxGareat { get; set; }
    }
}
