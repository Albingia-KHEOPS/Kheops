using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class NouvelleAffaire {
        public AffaireId Offre { get; set; }
        public string CodeContrat { get; set; }
        public int VersionContrat { get; set; }
        public List<NouvelleAffaireRisque> Risques { get; set; } = new List<NouvelleAffaireRisque>();
        public List<NouvelleAffaireFormule> Formules { get; set; } = new List<NouvelleAffaireFormule>();
    }
}
