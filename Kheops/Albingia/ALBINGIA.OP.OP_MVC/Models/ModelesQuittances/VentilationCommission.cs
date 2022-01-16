using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VentilationCommission
    {
        public string ModeAffichage { get; set; }
        public string NumQuittanceVisu { get; set; }
        public string NatureContrat { get; set; }
        public List<VentilationCommissionCourtier> Courtiers;
        public List<VentilationCommissionGarantie> Garanties;
    }
}