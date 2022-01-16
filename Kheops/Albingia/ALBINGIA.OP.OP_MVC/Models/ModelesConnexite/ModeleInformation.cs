using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleInformation
    {
        public string ContratTraite { get; set; }
        public string NumConnexiteInformation { get; set; }
        public long CodeObservationInformation { get; set; }
        public string ObservationInformation { get; set; }
        public long IdeConnexiteInformation { get; set; }
        public List<ModeleLigneConnexite> ConnexitesInformation { get; set; }
        public bool IsConnexiteReadOnly { get; set; }

    }
}