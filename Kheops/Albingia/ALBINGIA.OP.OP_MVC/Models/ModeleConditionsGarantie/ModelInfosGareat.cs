using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.ModeleConditionsGarantie {
    public class ModelInfosGareat {
        public string CodeTranche { get; set; }
        public decimal TauxTranche { get; set; }
        public string Tranche => TauxTranche != decimal.Zero ? $"{CodeTranche} - {TauxTranche.ToString("P2")}" : string.Empty;
        public decimal TauxFraisGeneraux { get; set; }
        public decimal TauxCommissions { get; set; }
        public decimal TauxRetenu { get; set; }
        public decimal PrimeTheorique { get; set; }
        public decimal? Prime { get; set; }
        public string CodeRegimeTaxe { get; set; }
    }
}