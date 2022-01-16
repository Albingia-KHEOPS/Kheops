using Albingia.Kheops.OP.Domain.Affaire;
using System;

namespace Albingia.Kheops.DTO {
    public class VerrouAffaire : LockState {
        public AffaireId AffaireId { get; set; }
        public DateTime Date => DateTime.Now;
    }
}
