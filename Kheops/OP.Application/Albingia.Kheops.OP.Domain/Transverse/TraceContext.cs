using Albingia.Kheops.OP.Domain.Affaire;

namespace Albingia.Kheops.OP.Domain {
    public class TraceContext {
        public AffaireId AffaireId { get; set; }
        public string User { get; set; }
        public string TypeTraitement { get; set; }
        public bool IsInfoSuspension { get; set; }
        public string TypeAction { get; set; }
        public int Ordre { get; set; }
        public string Libelle { get; set; }
    }
}
