using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Primes {
    public class RetardPaiement {
        public Contrat Contrat { get; set; }
        public Prime Prime { get; set; }
        public string DateValidationContrat { get; set; }
        public string DateLimite { get; set; }
        public string Courtier { get; set; }
    }
}