using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public class RegulRisque {
        public long Id { get; set; }
        public AffaireId AffaireId { get; set; }
        public long IdRegul { get; set; }
        public int Numero { get; set; }
    }
}
