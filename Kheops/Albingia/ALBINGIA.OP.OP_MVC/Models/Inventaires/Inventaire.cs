using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class Inventaire
    {
        public long Id { get; set; }

        public string Descriptif { get; set; }

        public int NumeroType { get; set; }

        public string CodeType { get; set; }

        public string LabelType { get; set; }

        public string Description { get; set; }

        public decimal Valeur { get; set; }

        public string UniteValeur { get; set; }

        public List<LabeledValue> UniteListe { get; set; }

        public string TypeValeur { get; set; }

        public List<LabeledValue> TypeListe { get; set; }

        public string TypeTaxe { get; set; }

        public List<LabeledValue> TaxeListe { get; set; }

        public bool ActiverReport { get; set; }

        public List<InventoryItem> Infos { get; set; } = new List<InventoryItem>();

        public List<LabeledValue> CodesExtensions { get; set; }
    }
}