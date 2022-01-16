using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.DTO {
    public class SelectionObjetRegularisationDto {
        public long IdLot { get; set; }
        public string Perimetre => "OB";
        public bool EnCours { get; set; } = true;
        public ObjetDto Objet { get; set; }
    }
}
