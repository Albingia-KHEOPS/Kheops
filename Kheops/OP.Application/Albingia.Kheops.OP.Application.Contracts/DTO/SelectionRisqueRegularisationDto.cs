using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class SelectionRisqueRegularisationDto {
        public long IdLot { get; set; }
        public string Perimetre => "RQ";
        public bool EnCours { get; set; } = true;
        public RisqueDto Risque { get; set; }
    }
}
