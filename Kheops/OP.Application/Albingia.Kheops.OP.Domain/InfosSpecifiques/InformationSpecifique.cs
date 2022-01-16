using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.InfosSpecifiques {
    public class InformationSpecifique {
        public AffaireId AffaireId { get; set; }
        public int NumeroRisque { get; set; }
        public int NumeroObjet { get; set; }
        public int NumeroOption { get; set; }
        public int NumeroFormule { get; set; }
        public string Cle { get; set; }
        public LigneModeleIS ModeleLigne { get; set; }
        public ValeurInformationSpecifique Valeur { get; set; }
    }
}
