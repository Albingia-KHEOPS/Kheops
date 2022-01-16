using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleRegularisation
    {
        public string ContratTraite { get; set; }
        public string NumConnexiteRegularisation { get; set; }
        public long CodeObservationRegularisation { get; set; }
        public string ObservationRegularisation { get; set; }
        public long IdeConnexiteRegularisation { get; set; }
        public List<ModeleLigneConnexite> ConnexitesRegularisation { get; set; }
        public bool IsConnexiteReadOnly { get; set; }
    }
}