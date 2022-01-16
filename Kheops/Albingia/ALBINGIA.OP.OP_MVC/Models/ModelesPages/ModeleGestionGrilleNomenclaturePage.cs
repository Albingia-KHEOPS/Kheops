using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestionNomenclature;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleGestionGrilleNomenclaturePage : MetaModelsBase
    {
        public List<ModeleGrille> Grilles { get; set; }
    }
}