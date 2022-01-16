using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamFiltres;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamFiltresPage : MetaModelsBase
    {
        public string AdditionalParam { get; set; }
        public List<ModeleLigneFiltre> ListeFiltres { get; set; }     
    }
}