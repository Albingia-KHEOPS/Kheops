using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public class SelectionRegularisation : Value {
        public AffaireId AffaireId { get; set; }
        public long IdLot { get; set; }
        virtual public PerimetreSelectionRegul Perimetre { get; set; }
        public bool? EnCours { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int NumeroRisque { get; set; }
        virtual public int NumeroObjet { get; set; }
        public int NumeroFormule { get; set; }
        public long IdGarantie { get; set; }
        public string CodeGarantie { get; set; }
        public long SequenceGarantie { get; set; }
        virtual public string TypeEdition { get; set; } = string.Empty;
    }
}
