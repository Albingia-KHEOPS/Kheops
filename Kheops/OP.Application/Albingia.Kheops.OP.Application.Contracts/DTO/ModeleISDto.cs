using Albingia.Kheops.OP.Domain.InfosSpecifiques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class ModeleISDto {
        public string Code { get; set; }
        public string CodeBranche { get; set; }
        public string CodeCible { get; set; }
        public TypeSection Section { get; set; }
        public IEnumerable<int> NumerosRisques { get; set; }
        public IEnumerable<LigneModeleISDto> Proprietes { get; set; }
    }
}
