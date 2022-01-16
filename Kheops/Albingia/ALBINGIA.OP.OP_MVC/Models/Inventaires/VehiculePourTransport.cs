using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class VehiculePourTransport : InventoryItem
    {
        public string Marque { get; set; }

        public string Modele { get; set; }

        public string Immatriculation { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public double Montant { get; set; }
    }
}