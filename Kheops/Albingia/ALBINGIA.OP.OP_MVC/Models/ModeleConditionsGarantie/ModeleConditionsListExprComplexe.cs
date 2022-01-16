using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie
{
    public class ModeleConditionsListExprComplexe
    {
        public bool IsReadOnly { get; set; }
        public string Type { get; set; }
        public string CodeExpr { get; set; }
        public string Mode { get; set; }
        public List<ModeleConditionsExprComplexeDetails> ListExpressions { get; set; }
    }
}