using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class Materiel : InventoryItem
    {
        public string Designation { get; set; }

        public double Valeur { get; set; }
    }
}