using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class PosteAssure : InventoryItem
    {
        public string Designation { get; set; }

        public double Montant { get; set; }
    }
}