using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModelePleinEcranConnexite
    {
        public string CodeTypeConnexite { get; set; }

        public string ContratTraite { get; set; }
        public string NumConnexite { get; set; }
       
        public long CodeObservation { get; set; }
        public string Observation { get; set; }
        public long IdeConnexite { get; set; }
        public bool IsConnexiteReadOnly { get; set; }
       
        public List<ModeleLigneConnexite> listeConnexite { get; set; }
    }
}