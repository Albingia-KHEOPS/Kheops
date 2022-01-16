using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Formule;
using System;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO
{
    public class OptionDto
    {
        public List<OptionsDetailVoletDto> OptionVolets { get; set; } = new List<OptionsDetailVoletDto>();
        public int NumeroFormule { get; set; }
        public AffaireId AffaireId { get; set; }
        public MontantsOption MontantsOption { get; set; }
        public long Id { get; set; }
        public int OptionNumber { get; set; }
        public int? NumeroAvenant { get; set; }
        public IEnumerable<ApplicationDto> Applications { get; set; }
        public bool IsModifiedForAvenant { get; set; }
        public DateTime? DateAvenantModif { get; set; }
    }
}