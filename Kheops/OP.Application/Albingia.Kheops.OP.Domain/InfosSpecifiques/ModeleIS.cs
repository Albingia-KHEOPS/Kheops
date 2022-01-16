using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.InfosSpecifiques {
    public class ModeleIS {
        public const string ToutesCibles = "TOUTESCIBLES";
        public string Code { get; set; }
        public string CodeBranche { get; set; }
        public string CodeCible { get; set; }
        public TypeSection Section { get; set; }
        public bool IsRecup { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public IEnumerable<LigneModeleIS> Proprietes { get; set; }
    }
}
