using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class VentilationDetaillee
    {
        public string ModeAffichage { get; set; }
        public string NumQuittanceVisu { get; set; }

        public string NatureContrat { get; set; }
        public string ComplementTitre { get; set; }
        public List<VentilationDetailleeGarantie> Garanties;
        public List<VentilationDetailleeTaxe> Taxes;
        public List<VentilationDetailleeRegime> RegimesTaxe;
    }
}