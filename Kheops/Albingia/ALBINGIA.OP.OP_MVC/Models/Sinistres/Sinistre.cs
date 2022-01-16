using ALBINGIA.Framework.Common;
using System;

namespace ALBINGIA.OP.OP_MVC.Models {
    public class Sinistre {
        public Contrat Contrat { get; set; }
        public ulong MontantObjet { get; set; }
        public string Situation { get; set; }
        public DateTime DateSurvenance { get; set; }
        public string Annee => DateSurvenance.ToString("yyyy");
        public DateTime DateOuverture { get; set; }
        public string Survenance => IgnorerJour? DateSurvenance.ToString("MMM yyyy") : DateSurvenance.ToShortDateString();
        public int Numero { get; set; }
        public string CodeSousBranche { get; set; }
        public bool IgnorerJour { get; set; }
        public string Ouverture => DateOuverture.ToShortDateString();
        public decimal ChargementTotal { get; set; }
    }
}