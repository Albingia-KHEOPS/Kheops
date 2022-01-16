using System;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie {
    public class Option
    {
        public int Numero { get; set; }

        public List<Volet> Volets { get; set; }

        public ICollection<Application> Applications { get; set; }

        public DateTime? DateAvenantModif { get; set; }

        public bool IsModifiedForAvenant { get; set; }

        public bool IsExpanded { get; set; }
    }
}