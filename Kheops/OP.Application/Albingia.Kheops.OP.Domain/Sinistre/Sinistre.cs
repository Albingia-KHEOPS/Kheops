using ALBINGIA.Framework.Common.Extensions;
using System;

namespace Albingia.Kheops.OP.Domain {
    public class Sinistre {
        public SinistreId Id { get; set; }
        public Affaire.Affaire Affaire { get; set; }
        public ulong MontantObjet { get; set; }
        public SituationSinistre Situation { get; set; }
        public bool IgnorerJour { get; set; }
        public DateTime DateOuverture { get; set; }
    }
}