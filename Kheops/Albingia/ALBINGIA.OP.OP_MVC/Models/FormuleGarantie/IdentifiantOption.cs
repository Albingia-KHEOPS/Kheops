using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie {
    public class IdentifiantOption {
        public Affaire Affaire { get; set; }
        public int NumOption { get; set; }
        public int NumFormule { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsHisto { get; set; }
    }
}
