using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models {
    public class RelanceOffre {
        public Offre Offre { get; set; }
        public DateTime DateValidation { get; set; }
        public string Date => DateValidation.ToShortDateString();
        public string Souscripteur { get; set; }
        public string Gestionnaire { get; set; }
        public string Courtier { get; set; }
        public int DelaisRelanceJours { get; set; }
        public string EcartDelais => $"J+{(DateTime.Today - DateValidation.AddDays(DelaisRelanceJours)).Days}";
        public string Situation { get; set; }
        public string MotifStatut { get; set; } = string.Empty;
        public DateTime DateRelance { get; set; }
        public bool IsAttenteDocCourtier { get; set; }
    }
}