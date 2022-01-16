using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCotisations
{
    [Serializable]
    public class ModeleTarif
    {
        public string CodeTarif { get; set; }
        public string LCIValeur { get; set; }
        public string LCIUnite { get; set; }
        public string FranchiseValeur { get; set; }
        public string FranchiseUnite { get; set; }
        public string TauxValeur { get; set; }
        public string TauxUnite { get; set; }
    }
}