using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Formule
{
    public class Application
    {
        public long Id { get; set; }

        public ApplicationNiveau Niveau { get; set; }

        public int NumRisque { get; set; }
        public int NumObjet { get; set; }

        internal void ResetIds() {
            Id = default;
        }
    }

    public enum ApplicationNiveau {
        [BusinessCode("RQ")] Risque,
        [BusinessCode("OB")] Objet,
    }
}
