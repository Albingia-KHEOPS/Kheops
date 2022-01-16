using System;

namespace ALBINGIA.OP.OP_MVC.Models {
    [Serializable]
    public class SelectionObjet {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public decimal Valeur { get; set; }
        public string Unite { get; set; }
        public string Type { get; set; }
    }
}