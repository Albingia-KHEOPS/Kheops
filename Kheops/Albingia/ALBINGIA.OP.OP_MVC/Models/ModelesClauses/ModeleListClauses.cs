using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleListClauses 
    {
        public List<ModeleClause> TableauClauses { get; set; }
        public int NbCount { get; set; }
        public int StartLigne { get; set; }
        public int EndLigne { get; set; }
        public int PageNumber { get; set; }
        public int LineCount { get; set; }
        public string Provenance { get; set; }
        public bool FullScreen { get; set; }
       
        
    }
}