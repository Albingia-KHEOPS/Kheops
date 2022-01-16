namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class Totaux
    {
        public decimal TotalHorsCatNatHT { get; set; }
        public decimal TotalHorsCatNatTaxe { get; set; }
        public decimal TotalHorsCatNatTTC { get; set; }

        public decimal GareatHT { get; set; }
        public decimal GareatTaxe { get; set; }
        public decimal GareatTTC { get; set; }

        public decimal CatNatHT { get; set; }
        public decimal CatNatTaxe { get; set; }
        public decimal CatNatTTC { get; set; }

        public decimal TotalHorsFraisHT { get; set; }
        public decimal TotalHorsFraisTaxe { get; set; }
        public decimal TotalHorsFraisTTC { get; set; }

        public decimal FraisHT { get; set; }
        public decimal FraisTaxe { get; set; }
        public decimal FraisTTC { get; set; }
        //public decimal FraisTTC { get { return FraisHT + FraisTaxe; } }

        public decimal FGATaxe { get; set; }
        public decimal FGATTC { get; set; }
        public decimal TotalHT { get; set; }

        public string TotalTaxe { get; set; }
        public string TotalTTC { get; set; }
        public string NumAvenantPage { get; set; }
    }
}