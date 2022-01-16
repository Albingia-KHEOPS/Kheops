using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamISReference;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamISReferencePage : MetaModelsBase
    {
        public List<LigneModeleIS> ListeReferentiels { get; set; }
        public LigneModeleIS LigneVide { get; set; }
    }
}