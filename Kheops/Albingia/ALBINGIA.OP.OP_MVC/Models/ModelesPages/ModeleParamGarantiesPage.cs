using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamGaranties;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamGarantiesPage : MetaModelsBase
    {
        public List<LigneGarantie> ListeGaranties { get; set; }
        public string AdditionalParam { get; set; }
    }
}