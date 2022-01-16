using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesCotisations
{
    [Serializable]
    public class ModeleCotisationUpdate
    {
        public string CodeGarantie { get; set; }
        public string Tarif { get; set; }
        public string TauxForce { get; set; }
        public string MontantForce { get; set; }
        public string CoefCom { get; set; }
        public string TotalHorsFraisHT { get; set; }
        public string TotalHT { get; set; }
        public string TotalTTC { get; set; }
        public string CommentForce { get; set; }
    }
}