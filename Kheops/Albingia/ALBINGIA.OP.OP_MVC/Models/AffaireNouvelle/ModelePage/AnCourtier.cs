using ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.Courtiers;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage
{
    public class AnCourtier : MetaModelsBase
    {
        public List<Courtier> Courtiers { get; set; }
        //public Single TotalCommission { get; set; }
        public int CourtierApporteur { get; set; }
        public int CourtierGestionnaire { get; set; }
        public int CourtierPayeur { get; set; }
        public string RisqueObj { get; set; } //Savoir si il y a un objet renseigné ou pas.
        public string ModeAffichage { get; set; }
        public string Contexte { get; set; }
        public CommissionCourtier CommissionsStandard { get; set; }
        public string modeAffiche { get; set; }

        public bool LoadedFromQuittance { get; set; }
    }
}