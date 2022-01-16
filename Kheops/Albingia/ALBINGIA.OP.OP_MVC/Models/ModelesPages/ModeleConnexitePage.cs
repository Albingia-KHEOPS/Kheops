using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesConnexite;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleConnexitePage : MetaModelsBase
    {
        public string CaractereSplit { get; set; }
        public ModeleRecherchePage Recherche { get; set; }
        public ModeleEngagement Engagement { get; set; }
        public ModeleRemplacement Remplacement { get; set; }
        public ModeleInformation Information { get; set; }
        public ModeleResiliation Resiliation { get; set; }
        public ModeleRegularisation Regularisation { get; set; }
        public string Branche { get; set; }
        public string SousBranche { get; set; }
        public string Categorie { get; set; }
        public bool IsConnexiteReadOnly { get; set; }
        public string TypeAffichage { get; set; }


        public ModeleInfosConnexite InfosConnexite { get; set; }
    }
}