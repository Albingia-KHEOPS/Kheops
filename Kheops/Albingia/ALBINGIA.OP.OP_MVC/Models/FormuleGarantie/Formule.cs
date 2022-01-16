using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie {
    public class Formule
    {
        private bool isAvnDisabled;
        private InfoApplication applications;

        public InfoApplication Applications {
            get {
                if (this.applications != null) {
                    this.applications.IsAvnDisabled = IsAvnDisabled;
                }
                return this.applications;
            }
            set {
                this.applications = value;
                if (this.applications is null) {
                    return;
                }
                this.applications.IsAvnDisabled = IsAvnDisabled;
            }
        }

        public DateTime? DateEffetAvenantContrat { get; set; }

        public int Numero { get; set; }

        public string Libelle { get; set; }

        public string Alpha { get; set; }

        public Risque Risque { get; set; }

        public int? SelectedOptionNumber { get; set; }

        public bool? IsSelected { get; set; }

        public IEnumerable<LabeledValue> ListeNatures { get; set; }

        public List<Option> Options { get; set; }

        public bool IsExpanded { get; set; }

        public bool IsAvnDisabled {
            get => this.isAvnDisabled;
            set {
                this.isAvnDisabled = value;
                if (Applications != null) {
                    Applications.IsAvnDisabled = value;
                }
            }
        }
    }
}