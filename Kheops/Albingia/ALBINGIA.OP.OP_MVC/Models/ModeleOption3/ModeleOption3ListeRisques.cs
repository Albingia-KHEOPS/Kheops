using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleOption3
{
    public class ModeleOption3ListeRisques
    {
        public string TableName { get; set; }
        public List<ModeleRisque> Risques { get; set; }
    }
}