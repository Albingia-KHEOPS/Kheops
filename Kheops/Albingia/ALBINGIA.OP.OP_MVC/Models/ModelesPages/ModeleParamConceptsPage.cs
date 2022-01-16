using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamConcepts;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamConceptsPage : MetaModelsBase
    {
        public List<ModeleLigneConcept> ListeConcepts { get; set; }        
        public string AdditionalParam { get; set; }
    }
}