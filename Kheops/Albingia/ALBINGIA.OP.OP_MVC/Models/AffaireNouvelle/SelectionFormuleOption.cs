using System;

namespace ALBINGIA.OP.OP_MVC.Models {
    [Serializable]
    public class SelectionFormuleOption {
        public int NumFormule { get; set; }
        public string NomFormule { get; set; }
        public int NumOption { get; set; }
        public string Application { get; set; }
        public bool Selected { get; set; }
    }
}