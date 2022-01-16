using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.AutoComplete {
    public class Preneur {
        public int Code { get; set; }
        public string Nom { get; set; }
        public string[] NomSecondaires { get; set; }
        public string Departement { get; set; }
        public string Ville { get; set; }
        public string VilleCedex { get; set; }
        public string SIREN { get; set; }
    }
}