using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public abstract class Marchandises : InventaireItem
    {
        public string Nature { get; set; }

        public decimal Montant { get; set; }
    }
}