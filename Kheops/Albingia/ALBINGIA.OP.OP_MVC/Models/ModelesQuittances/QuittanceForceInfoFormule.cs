using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesQuittances
{
    public class QuittanceForceInfoFormule
    {
        public Int64 CodeFor { get; set; }
        public string FormLettre { get; set; }
        public string FormDesc { get; set; }
        public int CodeRsq { get; set; }
        public string RsqDesc { get; set; }
        public double MontantCal { get; set; }
        public double MontantForce { get; set; }
        public int NumeroFormule { get; set; }
    }
}