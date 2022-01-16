using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamISAssocier;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamISModelesPage : MetaModelsBase
    {
        public ParamISDrlModele ListeModelesISModele { get; set; }
        //public List<AlbSelectListItem> ListeModelesIS { get; set; }
        public ParamISModele SelectedModele { get; set; }             
    }
}