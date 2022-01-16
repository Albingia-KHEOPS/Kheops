using Albingia.Kheops.OP.Domain.Affaire;
using Albingia.Kheops.OP.Domain.Referentiel;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO
{
    public class FormuleDto
    {
        public AffaireId AffaireId { get; set; }
        public RisqueDto Risque { get; set; }
        public int Chrono { get; set; }
        public int FormuleNumber { get; set; }
        public string Alpha { get; set; }
        public string Description { get; set; }
        public CibleCatego Cible { get; set; }
        public long Id { get; set; }
        public int? SelectedOptionNumber { get; set; }
        public bool? IsSelected { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
    }
}