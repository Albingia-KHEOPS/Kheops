using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.AutoComplete {
    public class Courtier : Preneur {
        public int CodePostal { get; set; }
        public bool EstValide { get; set; }
    }
}