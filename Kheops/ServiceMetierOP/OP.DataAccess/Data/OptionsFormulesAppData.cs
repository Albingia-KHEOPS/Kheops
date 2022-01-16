using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess.Data {
    public class OptionsFormulesAppData : AffaireBaseData {
        public int For { get; set; }
        public int Opt { get; set; }
        public int Rsq { get; set; }
        public int Obj { get; set; }
        public string LibFor { get; set; }
        public string LibRsq { get; set; }
        public string LibObj { get; set; }
    }
}
