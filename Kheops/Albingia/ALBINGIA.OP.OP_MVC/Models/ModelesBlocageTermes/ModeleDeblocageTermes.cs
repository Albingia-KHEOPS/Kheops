using System;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesBlocageTermes
{
    public class ModeleDeblocageTermes
    {
        public DateTime? DebutPeriodeEcheance { get; set; }
        public DateTime? FinPeriodeEcheance { get; set; }
        public DateTime? DateEmissionEcheance { get; set; }
        public string MsgErreur { get; set; }


    }
}