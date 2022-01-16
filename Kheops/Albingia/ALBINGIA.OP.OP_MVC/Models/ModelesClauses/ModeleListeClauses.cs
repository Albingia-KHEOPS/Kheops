using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleListeClauses
    {
        public string SelectionPossible { get; set; }
        public string ModaliteAffichage { get; set; }
        public int Date { get; set; }
        public List<ModeleClausier> Clauses { get; set; }
        public bool FullScreen { get; set; }
    }
}