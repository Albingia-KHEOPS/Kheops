using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie {
    public class IdentifiantGarantie : IdentifiantOption {
        public string CodeBloc { get; set; }
        public long Sequence { get; set; }
    }
}
