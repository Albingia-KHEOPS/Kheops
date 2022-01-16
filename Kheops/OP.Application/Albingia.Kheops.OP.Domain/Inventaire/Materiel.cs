using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public abstract class Materiel : InventaireItem
    {
        public string Designation { get; set; }

        public Decimal Valeur { get; set; }
    }
}