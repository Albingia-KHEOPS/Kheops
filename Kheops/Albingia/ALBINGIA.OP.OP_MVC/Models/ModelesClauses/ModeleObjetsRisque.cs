using ALBINGIA.OP.OP_MVC.Models.ModelesObjet;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleObjetsRisque
    {
        public string CodeRisque { get; set; }
        public string DescriptifRisque { get; set; }
        public List<ModeleObjet> Objets { get; set; }
    }
}