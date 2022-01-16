using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire {
    public class CotisationOffre {
        public string CodeOffre { get; set; }
        public int VersionOffre { get; set; }
        public decimal TauxCommission { get; set; }
    }
}
