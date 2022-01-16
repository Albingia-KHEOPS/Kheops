using OP.WSAS400.DTO.Traite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class EngagementTraiteDto {
        public string CodeTraite { get; set; }
        public string CodePeriode { get; set; }
        public TraiteDto Traite { get; set; }
    }
}
