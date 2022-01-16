using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire {
    public class TauxGareat {
        public decimal Valeur { get; set; }
        public decimal Taxe { get; set; }
        public decimal Commission { get; set; }
        public decimal FraisGestion { get; set; }
        public decimal Cession { get; set; }
        public decimal Total { get; set; }
    }
}
