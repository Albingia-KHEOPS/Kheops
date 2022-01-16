using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;

namespace Albingia.Kheops.OP.Domain.Risque {
    public class Risque
    {
        public AffaireId AffaireId { get; set; }
        public int Numero { get; set; }
        public string Designation { get; set; }
        public Cible Cible { get; set; }
        public Branche Branche { get; set; }
        public string SousBranche { get; set; }

        public ICollection<Objet> Objets { get; set; } = new List<Objet>();
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int NumeroAvenantCreation { get; set; }
        public int NumeroAvenantModification { get; set; }
        public DateTime? DateEffetAvenant { get; set; }
        public bool IsTemporaire { get; set; }
        public bool ParticipationBeneficiaire { get; set; }
        public bool BonnificationNonSinistre { get; set; }
        public bool ARegulariser { get; set; }
        public bool BonnificationNonSinistreIncendie { get; set; }
        public TarifGeneral TarifLCI { get; set; }
        public TarifGeneral TarifFranchise { get; set; }
        public RegimeTaxe RegimeTaxe { get; set; }
        public bool AllowCANAT { get; set; }
        public string Nomenclature01 { get; set; }

        public bool IsFinishingAfter(DateTime? dateModif)
        {
            return (!dateModif.HasValue || !DateFin.HasValue || DateFin >= dateModif);
        }

        public void ChangeTarifsConditions(TarifGeneral newTarifLCI, TarifGeneral newTarifFranchise) {
            TarifLCI.Reset(newTarifLCI);
            TarifFranchise.Reset(newTarifFranchise);
        }
    }
}
