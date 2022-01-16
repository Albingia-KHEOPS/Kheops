using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Risque
{
    public class Objet
    {
        public ObjetId Id { get; set; }
        public long Designation { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Unite { get; set; }
        public long Valeur { get; set; }

        public bool IsFinishingAfter(DateTime? dateModif) {
            return (!dateModif.HasValue || !DateFin.HasValue || DateFin >= dateModif);
        }

        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int NumeroAvenantCreation { get; set; }
        public int NumeroAvenantModification { get; set; }
        public DateTime? DateEffetAvenant { get; set; }
        public Cible Cible { get; set; }
        public string Nomenclature01 { get; set; }
        public TarifGeneral TarifLCI { get; set; }
        public TarifGeneral TarifFranchise { get; set; }

        public void ChangeTarifsConditions(TarifGeneral newTarifLCI, TarifGeneral newTarifFranchise) {
            TarifLCI.Reset(newTarifLCI);
            TarifFranchise.Reset(newTarifFranchise);
        }
    }
}
