using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class NouvelleAffaireFormule {
        public AffaireId AffaireId { get; set; }
        public bool IsSelected { get; set; }
        public int? SelectedOptionNumber { get; set; }
        public int Numero { get; set; }
        public IDictionary<int, int> TarifGaranties { get; set; } = new Dictionary<int, int>();

        internal void SetSelectionForNewAffair(Formule.Formule formule) {
            if (formule != null) {
                formule.SelectedOptionNumber = SelectedOptionNumber;
                formule.IsSelected = IsSelected;
            }
        }
    }
}
