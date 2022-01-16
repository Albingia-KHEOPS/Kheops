using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public abstract class VoletBase : OptionItem
    {
        public string Type { get; set; }

        public bool IsCollapsed { get; set; }

        public bool AvenantModifie { get; set; }

        public bool IsHidden { get; set; }
    }
}