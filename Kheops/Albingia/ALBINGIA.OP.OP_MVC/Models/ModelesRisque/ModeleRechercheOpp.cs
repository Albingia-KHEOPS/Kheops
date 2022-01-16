using ALBINGIA.OP.OP_MVC.Models.ModeleTransverse;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRisque
{
    public class ModeleRechercheOpp
    {
        public List<ModeleOrganisme> lstOppositions;
        public string Code { get; set; }
        public string Nom { get; set; }
        public string context { get; set; }
      
    }
}