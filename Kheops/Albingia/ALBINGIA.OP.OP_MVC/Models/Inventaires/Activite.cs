using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class Activite : InventoryItem
    {
        public LabeledValue Code { get; set; }

        public double ChiffreAffaire { get; set; }

        public decimal PourcentageChiffreAffaire { get; set; }
    }
}