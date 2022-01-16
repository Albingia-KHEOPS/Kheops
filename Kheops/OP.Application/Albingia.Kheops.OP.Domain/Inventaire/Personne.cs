using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public abstract class Personne : InventaireItem
    {
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Fonction { get; set; }

        public DateTime? DateNaissance { get; set; }
    }
}