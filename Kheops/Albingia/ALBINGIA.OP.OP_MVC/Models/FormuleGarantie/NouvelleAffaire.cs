using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie {
    public class NouvelleAffaire {
        public string Code { get; set; }
        public int Version { get; set; }
        public string TabGuid { get; set; }
        public Folder Offre { get; set; }
        public List<Formule> Formules { get; set; }
        public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
    }
}