using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public abstract class Activite : InventaireItem
    {
        public virtual RefValue Code {get;set;}

        public decimal ChiffreAffaire { get; set; }

        public decimal PourcentageChiffreAffaire { get; set; }
    }
}