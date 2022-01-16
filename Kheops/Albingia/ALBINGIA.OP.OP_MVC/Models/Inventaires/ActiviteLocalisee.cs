using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class ActiviteLocalisee : InventoryItem
    {
        public string Designation { get; set; }

        public string Lieu { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public LabeledValue NatureLieu { get; set; }
    }
}