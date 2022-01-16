using Albingia.Kheops.Common;
using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;

namespace Albingia.Kheops.DTO
{
    public class AffaireFormuleDto
    {
        public AffaireFormuleDto()
        {
            this.Formules = new List<FormuleDto>();
        }
        public List<FormuleDto> Formules { get; set; }
        public AffaireId AffaireId { get; set; }
        public DateTime? DateEffetAvenant { get; set; }
        public List<ValidationError> ValidationErrors { get; } = new List<ValidationError>();
    }
}