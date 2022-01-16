using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class Marchandises : InventoryItem
    {
        public string Nature { get; set; }

        public double Montant { get; set; }
    }
}