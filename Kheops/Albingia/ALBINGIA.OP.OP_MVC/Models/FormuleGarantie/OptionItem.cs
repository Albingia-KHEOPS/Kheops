using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public abstract class OptionItem
    {
        public long UniqueId { get; set; }

        public string Code { get; set; }

        public bool IsChecked { get; set; }

        public string Libelle { get; set; }

        public LabeledValue Caractere { get; set; }
    }
}
