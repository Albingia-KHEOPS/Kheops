
using Albingia.Kheops.OP.Domain;
using System;

namespace Albingia.Kheops.DTO {
    public class SinistreDto {
        public AffaireDto Affaire { get; set; }
        public ulong MontantObjet { get; set; }
        public string Situation { get; set; }
        public DateTime DateSurvenance { get; set; }
        public DateTime DateOuverture { get; set; }
        public int Numero { get; set; }
        public string CodeSousBranche { get; set; }
        public bool IgnorerJour { get; set; }
        public CalculChargementDto CalculChargement { get; set; }
    }
}
