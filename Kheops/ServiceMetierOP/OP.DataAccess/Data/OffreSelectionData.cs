using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess.Data {
    public class OffreSelectionData : AffaireBaseData {
        public decimal Val { get; set; }
        public string Unit { get; set; }
        public string TypeVal { get; set; }
        public string Desc { get; set; }
        public int Rsq { get; set; }
        public string Sel { get; set; }
        public int Obj { get; set; }
        public string Pog { get; set; }
        public int Alg { get; set; }
        public DateTime? Deb { get; set; }
        public DateTime? Fin { get; set; }
    }
}
