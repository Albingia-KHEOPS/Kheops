using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class MarchandisesTransportees : Marchandises
    {
        public string Depart { get; set; }

        public string Destination { get; set; }

        public string Modalites { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }
    }
}