using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Transverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class PrimeGarantie {
        public AffaireId AffaireId { get; set; }
        public string TypePart { get; set; }
        public Montant MontantDevise { get; set; }
        public Montant MontantComptable { get; set; }
        public string CodeGarantie { get; set; }
    }
}
