using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class InventoryItem
    {
        public long ItemId { get; set; }
        public bool IsNew { get; set; }
        public int LineNumber { get; set; }
    }
}