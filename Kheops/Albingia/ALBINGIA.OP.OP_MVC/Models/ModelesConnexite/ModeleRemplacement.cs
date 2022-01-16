using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleRemplacement
    {
        public string ContratTraite { get; set; }
        public string NumConnexiteRemplacement { get; set; }
        public long CodeObservationRemplacement { get; set; }
        public string ObservationRemplacement { get; set; }
        public List<ModeleLigneConnexite> ConnexitesRemplacement { get; set; }
        public bool IsConnexiteReadOnly { get; set; }

    }
}