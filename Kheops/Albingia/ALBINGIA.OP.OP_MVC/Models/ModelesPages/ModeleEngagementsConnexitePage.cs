using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleEngagements;
using OP.WSAS400.DTO.Offres.Parametres;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleEngagementsConnexitePage : MetaModelsBase
    {
        public ModeleEngagement ModeleEngagement { get; set; }
        public List<ModeleContratConnexe> ContratsConnexes { get; set; }
        public List<ModeleTraite> AllTraitesContratsConnexes { get; set; }
        public decimal[] TotalPartAlbingia { get; set; }
        public decimal[] TotalPartTotale { get; set; }
        public string NumConnexite { get; set; }
        public List<ParametreDto> ListCodeTraites { get; set; }
        public string IdeConnexiteEngagement { get; set; }
        public ModelConnexites Connexites { get; set; }
    }
}