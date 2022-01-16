using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class ModeleSelectionDestinataire
    {
        public string GuidIdDestinataire { get; set; }
        public string TypeDestinataires { get; set; }
        public string TypeIntervenant { get; set; }
        public List<LigneSelectionDestinataire> ListeDestinataires { get; set; }
        public LigneSelectionDestinataire AutreDestinataire { get; set; }
    }
}