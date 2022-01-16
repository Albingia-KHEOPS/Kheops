using ALBINGIA.Framework.Business;
using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Connexites {
    public class FusionConnexites {
        public Folder Affaire { get; set; } = new Folder();
        public Folder AffaireCible { get; set; } = new Folder();
        public TypeConnexite TypeConnexite { get; set; }
        public string Observations { get; set; } = string.Empty;
        public List<ConnexiteBase> Connexites { get; set; } = new List<ConnexiteBase>();
        public List<ConnexiteBase> ConnexitesExternes { get; set; } = new List<ConnexiteBase>();
    }
}