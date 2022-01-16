using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Primes {
    public class Prime {
        public int Numero { get; set; }
        public int NumeroEcheance { get; set; }
        public decimal Montant { get; set; }
        public string DateEcheance { get; set; }
        public string LibelleRelance { get; set; }
    }
}