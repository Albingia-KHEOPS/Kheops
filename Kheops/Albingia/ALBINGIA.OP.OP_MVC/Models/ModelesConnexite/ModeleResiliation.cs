using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleResiliation
    {
        public string ContratTraite { get; set; }
        public string NumConnexiteResiliation { get; set; }
        public long CodeObservationResiliation { get; set; }
        public string ObservationResiliation { get; set; }
        public long IdeConnexiteResiliation { get; set; }
        public List<ModeleLigneConnexite> ConnexitesResiliation { get; set; }
        public bool IsConnexiteReadOnly { get; set; }

    }
}