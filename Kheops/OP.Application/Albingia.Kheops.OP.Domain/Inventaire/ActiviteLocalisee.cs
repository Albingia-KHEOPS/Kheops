using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public abstract class ActiviteLocalisee : InventaireItem
    {
        public string Designation { get; set; }

        public string Lieu { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public NatureLieu NatureLieu { get; set; }
    }
}