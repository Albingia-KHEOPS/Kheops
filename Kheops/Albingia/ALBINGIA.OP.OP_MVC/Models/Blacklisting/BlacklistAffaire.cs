using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models {
    public class BlacklistAffaire {
        public Blacklisting.Affaire Affaire { get; set; }
        public string Etat { get; set; }
        public string Situation { get; set; }
        public string Cible { get; set; }
        public string DateMAJ { get; set; }
        public string Preneur { get; set; }
        public string Courtier { get; set; }
        public string Qualite { get; set; }
    }
}