using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamTemplateNomenclature;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamTemplateNomenclaturePage : MetaModelsBase
    {
        public List<ModeleLigneTemplateNomenclature> ListeNomenclatures { get; set; }
    }
}