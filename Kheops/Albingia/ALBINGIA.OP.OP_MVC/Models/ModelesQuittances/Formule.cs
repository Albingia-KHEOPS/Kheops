using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    [Serializable]
    public class Formule
    {
        public string Libelle { get; set; }
        public string Risque { get; set; }
        public string HtHorsCatnat { get; set; }
        public string CatNat { get; set; }
        public string Taxes { get; set; }
        public string Ttc { get; set; }
        public string FinEffet { get; set; }
        
    }
}