using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class SimpleTrace {
        public AffaireId AffaireId { get; set; }
        public string Etape { get; set; }
        public int Id { get; set; }
        public int Formule { get; set; }
        public string CodeGarantie { get; set; }
        public int Objet{ get; set; }
        public int Option{ get; set; }
        public string Perimetre{ get; set; }
        public int Risque{ get; set; }
        public SituationAffaire Situation { get; set; }
        public DateTime? DateSituation { get; set; }

        public DateTime DateCreation { get; set; }
        public string Commentaire { get; set; }
        public string User { get; set; }
    }
}
