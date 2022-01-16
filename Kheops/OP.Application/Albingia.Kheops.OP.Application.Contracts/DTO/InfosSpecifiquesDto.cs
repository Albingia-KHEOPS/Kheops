using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class InfosSpecifiquesDto {
        public ModeleISDto Modele { get; set; }
        public IEnumerable<InformationSpecifiqueDto> Infos { get; set; }
        public bool ShouldRestoreOldValues { get; set; }
    }
}
