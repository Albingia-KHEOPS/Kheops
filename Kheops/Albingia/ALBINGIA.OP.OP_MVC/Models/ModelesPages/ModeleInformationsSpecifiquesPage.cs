using ALBINGIA.OP.OP_MVC.Models.MetaModels;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleInformationsSpecifiquesPage : MetaModelsBase
    {
        public string CodeRisque { get; set; }
        public string DescriptifRisque { get; set; }
        public ModeleInformationsSpecifiquesRisquesPage ModeleInformationsSpecifiquesRisquesPage { get; set; }
        public ModeleInformationsSpecifiquesRsqObjGarObjetPage ModeleInformationsSpecifiquesRsqObjGarObjetsPage { get; set; }
        public ModeleInformationsSpecifiquesRsqObjGarGarantiePage ModeleInformationsSpecifiquesRsqObjGarGarantiePage { get; set; }
    }
}