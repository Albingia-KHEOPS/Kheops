using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class SectionISDto {
        public TypeSection Type { get; set; }
        public int NumeroRisque { get; set; }
        public int NumeroObjet { get; set; }
        public int NumeroOption { get; set; }
        public int NumeroFormule { get; set; }
        public string Branche { get; set; }
    }
}
