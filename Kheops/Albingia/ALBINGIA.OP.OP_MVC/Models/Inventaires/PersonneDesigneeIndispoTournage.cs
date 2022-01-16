using System;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires {
    /// <summary>
    /// Same class as <see cref="PersonneDesigneeIndispo"/> to avoid mapster auto convertion (AllowImplicitSourceInheritance has no effect)
    /// </summary>
    public class PersonneDesigneeIndispoTournage: Personne {
        public PersonneDesigneeIndispoTournage() {
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