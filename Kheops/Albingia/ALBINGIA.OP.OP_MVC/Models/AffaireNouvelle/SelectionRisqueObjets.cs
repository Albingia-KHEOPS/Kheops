using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models {
    [Serializable]
    public class SelectionRisqueObjets {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public bool IsExpanded { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public decimal Valeur { get; set; }
        public string Unite { get; set; }
        public string Type { get; set; }
        public List<SelectionObjet> Objets { get; set; }
    }
}