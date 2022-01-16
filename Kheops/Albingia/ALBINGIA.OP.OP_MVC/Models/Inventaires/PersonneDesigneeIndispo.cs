using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires {
    public class PersonneDesigneeIndispo: Personne {
        public PersonneDesigneeIndispo() {
            IsNew = false;
            ItemId = 0;
            LineNumber = 0;
            Nom = string.Empty;
            Prenom = string.Empty;
            Fonction = string.Empty;
            DateNaissance = DateTime.MinValue;
            CodeExtension = string.Empty;
            Franchise = string.Empty;
            Montant = 0;
        }

        public string CodeExtension { get; set; }

        public string Franchise { get; set; }

        public double Montant { get; set; }
    }
}