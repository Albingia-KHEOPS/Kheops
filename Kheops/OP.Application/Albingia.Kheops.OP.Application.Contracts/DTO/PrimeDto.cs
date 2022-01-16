
using Albingia.Kheops.OP.Domain.Referentiel;
using System;

namespace Albingia.Kheops.DTO {
    public class PrimeDto {
        public int Numero { get; set; }
        public int NumeroEcheance { get; set; }
        public decimal Montant { get; set; }
        public DateTime DateEcheance { get; set; }
        public TypeRelance TypeRelance { get; set; }
    }
}
