using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Formule {
    public class GarantieUniqueId {
        public AffaireId AffaireId { get; set; }
        public long Sequence { get; set; }
    }
}
