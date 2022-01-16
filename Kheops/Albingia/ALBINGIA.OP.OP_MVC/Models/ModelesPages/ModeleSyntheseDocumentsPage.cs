using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesSyntheseDocuments;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleSyntheseDocumentsPage : MetaModelsBase
    {
        public List<SyntheseDocumentsDoc> Documents { get; set; }
    }
}