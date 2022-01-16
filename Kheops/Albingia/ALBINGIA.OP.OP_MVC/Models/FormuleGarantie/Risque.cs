using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie
{
    public class Risque {
        public int Numero { get; set; }
        public string Designation { get; set; }
        public ICollection<ObjetRisque> Objets { get; set; } = new List<ObjetRisque>();
        public LabeledValue Cible { get; set; }
        public LabeledValue Branche { get; set; }
        public DateTime? DateFin { get; set; }
        public bool? IsApplicable {
            get {
                return (Objets == null || Objets.Any(o => !o.IsApplique.HasValue)) ? null : (bool?)Objets.Any(o => o.IsApplique == false);
            }
        }
    }
}