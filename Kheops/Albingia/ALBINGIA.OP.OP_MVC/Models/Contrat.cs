using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models {
    public class Contrat : Offre {
        public int NumeroAvenant { get; set; }
        public DateTime? DateEffet { get; set; }
        public DateTime? DateEffetReelle { get; set; }
        public string Effet => DateEffet?.ToShortDateString() ?? string.Empty;
    }
}