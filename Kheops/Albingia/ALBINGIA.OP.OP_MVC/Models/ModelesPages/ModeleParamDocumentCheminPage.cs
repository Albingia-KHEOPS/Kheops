using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamDocumentChemin;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamDocumentCheminPage : MetaModelsBase
    {
        public List<ModeleLigneDocumentChemin> ListeChemins { get; set; }
    }
}