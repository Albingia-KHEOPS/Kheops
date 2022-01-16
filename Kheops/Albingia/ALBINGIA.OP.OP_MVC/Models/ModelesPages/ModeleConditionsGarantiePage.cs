using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleConditionsGarantiePage : MetaModelsBase
    {
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }
        public string CodeRisque { get; set; }
        
        public ModeleConditionsInfosContrat InformationsContrat { get; set; }
        
        public ModeleConditionsInfosCondition InformationsCondition { get; set; }

        public ModelInfosGareat InfosGareat { get; set; }

        public Framework.Common.Constants.NomsInternesEcran NomEcran { get; set; }
    }
}