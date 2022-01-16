using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class Personne : InventoryItem
    {
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Fonction { get; set; }

        public DateTime DateNaissance { get; set; }
    }
}