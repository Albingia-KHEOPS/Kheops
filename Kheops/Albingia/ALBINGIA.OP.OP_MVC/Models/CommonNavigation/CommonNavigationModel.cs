using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesPages;
namespace ALBINGIA.OP.OP_MVC.Models.CommonNavigation
{
    public class CommonNavigationModel : MetaModelsBase
    {
        public string Contexte { get; set; }
        public string Parametres { get; set; }
        public string CodeContrat { get; set; }
        public string TypeContrat { get; set; }
        public string VersionContrat { get; set; }

        public ModeleConnexitePage Connexite { get; set; }
    }
}
