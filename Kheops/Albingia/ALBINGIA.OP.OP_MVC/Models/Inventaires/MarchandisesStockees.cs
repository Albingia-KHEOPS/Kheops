using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class MarchandisesStockees : Marchandises
    {
        public string Lieu { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public LabeledValue Pays { get; set; }
    }
}