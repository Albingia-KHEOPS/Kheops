using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    [Serializable]
    public class Commission
    {
        public string TauxStd { get; set; }
        public decimal MontantStd { get; set; }
        public decimal MontantStdTotal { get { return MontantStd + MontantStdCatNat; } }
        public decimal MontantStdCatNat { get; set; }
        public string TauxStdCatNat { get; set; }

        public string TauxCatNatRetenu { get; set; }        
        public decimal MontantCatNatRetenu { get; set; }                
        public string TauxHRCatNatRetenu { get; set; }
        public decimal MontantHRCatNatRetenu { get; set; }
        public decimal MontantTotalRetenu { get; set; }
        //public decimal MontantRetenuTotal { get { return MontantRetenu + MontantRetenuCatNat; } }

        ///Modification FDU 2015-04-28
        public string Periodicite { get; set; }
        public DateTime? ProchaineEcheance { get; set; }
        public Double MontantHTHRCN { get; set; }
        public Double MontantCN { get; set; }
        public string TypeAvt { get; set; }
        public bool TraceCC { get; set; }

        public string ActeGestionRegule { get; set; }

    }
}