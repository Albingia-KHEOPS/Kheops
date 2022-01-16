using System;

namespace Albingia.Kheops.DTO {
    public class PeriodeGarantieDto {
        public long Id { get; set; }
        public int NumeroRisque { get; set; }
        public int NumeroFormule { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal Base { get; set; }
        public decimal BaseRegul { get; set; }
        public decimal Definitif2 { get; set; }
        public decimal Definitif2Prime { get; set; }
        public decimal Definitif2Taux { get; set; }
        public string UniteTauxBase { get; set; }
        public string UniteTaux2 { get; set; }
        public decimal Assiette { get; set; }
        public decimal TauxBase { get; set; }
        public decimal ValeurHT { get; set; }
        public string UniteTauxHT { get; set; }
        public decimal MontantHTHorsCATNAT { get; set; }
        public bool IsValidated { get; set; }
        public string CodeGarantie { get; set; }
    }
}
