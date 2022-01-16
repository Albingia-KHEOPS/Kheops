using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public class Regularisation {
        public AffaireId AffaireId { get; set; }
        public long Id { get; set; }
        public int Exercice { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public DateTime DateAvenant { get; set; }
        public int IdCourtier { get; set; }
        public int IdCourtierCommission { get; set; }
        public decimal TauxCommission { get; set; }
        public decimal TauxCommissionCATNAT { get; set; }
        public MotifRegularisation Motif { get; set; }
        public Quittancement Encaissement { get; set; }
        public SituationRegularisation Situation { get; set; }
        public ModeRegularisation Mode { get; set; }
        public NiveauRegularisation Niveau { get; set; }
        public bool SuivieAvenant { get; set; }
        public int MontantFraisAcc { get; set; }
        public string CodeApplicationFraisAcc { get; set; }
        public string CodeEtat { get; set; }
        public bool IsAvenant { get; set; }
    }
}
