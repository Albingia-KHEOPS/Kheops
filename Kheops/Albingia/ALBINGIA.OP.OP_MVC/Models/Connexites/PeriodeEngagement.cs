using System;
using System.Collections.Generic;

namespace ALBINGIA.OP.OP_MVC.Models.Connexites {
    public class PeriodeEngagement {
        public PeriodeEngagement() {
            Id = 0;
            IsUsed = false;
            Valeurs = new List<ValeurEngagement>();
            Ordre = 1;
        }
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public bool IsInactive { get; set; }
        public bool IsEnCours { get; set; }
        public DateTime? Beginning { get; set; }
        public DateTime? End { get; set; }
        public List<ValeurEngagement> Valeurs { get; set; }
        public int Ordre { get; set; }
    }
}
