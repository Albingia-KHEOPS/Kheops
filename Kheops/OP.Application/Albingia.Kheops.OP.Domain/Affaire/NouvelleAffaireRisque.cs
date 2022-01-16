using System.Collections.Generic;

namespace Albingia.Kheops.OP.Domain {
    public class NouvelleAffaireRisque {
        public int Numero { get; set; }
        public bool IsSelected { get; set; }
        public List<NouvelleAffaireObjet> Objets { get; set; } = new List<NouvelleAffaireObjet>();
    }
}