using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class VehiculePourTransport : InventaireItem
    {
        public string Marque { get; set; }

        public string Modele { get; set; }

        public string Immatriculation { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public Decimal Montant { get; set; }
    }
}