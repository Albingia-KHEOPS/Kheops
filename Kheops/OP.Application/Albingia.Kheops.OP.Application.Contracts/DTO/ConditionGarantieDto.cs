using Albingia.Kheops.OP.Domain.Formule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class ConditionGarantieDto {
        public int IdGarantie { get; set; }
        public long IdOption { get; set; }
        public int NumeroRisque { get; set; }
        public string CodeGarantie { get; set; }
        public ValeursOptionsTarif AssietteGarantie { get; set; }
        public TarifGarantieDto TarifsGarantie { get; set; }
    }
}
