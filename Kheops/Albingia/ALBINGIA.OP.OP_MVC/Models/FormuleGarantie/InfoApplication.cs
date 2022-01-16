using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.FormuleGarantie {
    public class InfoApplication {
        public bool IsModeOffre { get; set; }
        public int NumFormule { get; set; }
        public ICollection<Risque> Risques { get; set; }
        public string LettreAlpha { get; set; }
        public string LibelleFormule { get; set; }
        public int NumRisque { get; set; }

        /// <summary>
        /// Gets or sets Objets Appliqués if Application is not for the whole Risque
        /// </summary>
        public int[] NumsObjets { get; set; }

        public string LibelleApplication { get; set; }
        public DateTime? DateAvenantOption { get; set; }
        public bool IsAvnDisabled { get; set; }
        public void InitializeLibelleApplication() {
            var rsq = NumRisque > 0 && Risques?.Any() == true ? Risques.Single(r => r.Numero == NumRisque) : new Risque();
            LibelleApplication = string.Empty;
            if (rsq.Numero > 0) {
                bool isSingleObjApp = NumsObjets?.Count() == 1;
                bool isObjsApp = (NumsObjets?.Count() ?? 0) > 1;
                bool isMultiRisques = Risques.Count > 1;
                if (!isSingleObjApp && !isObjsApp && !isMultiRisques) {
                    LibelleApplication= "à l'ensemble du risque";
                }
                else if (!isSingleObjApp && !isObjsApp) {
                    LibelleApplication = $"au risque {rsq.Numero} '{rsq.Designation}'";
                }
                else {
                    LibelleApplication = (isSingleObjApp
                        ? $"à l'objet {NumsObjets.First()}"
                        : $"aux objets {string.Join(", ", NumsObjets)}") + " du risque";

                    if (isMultiRisques) {
                        LibelleApplication += $" {rsq.Numero} '{rsq.Designation}'";
                    }
                }
            }
        }

        public bool HasAllObjetsSortis(DateTime date) {
            if (Risques?.Any(r => r.Numero == NumRisque && r.Objets?.Any() == true) != true) {
                return false;
            }
            var risque = Risques.First(r => r.Numero == NumRisque);
            return risque.Objets
                .Where(o => o.IsApplique == true && ((!NumsObjets?.Any() ?? true) || NumsObjets.Contains(o.Code)))
                .All(o => (o.DateFin ?? risque.DateFin) < date);
        }
    }
}