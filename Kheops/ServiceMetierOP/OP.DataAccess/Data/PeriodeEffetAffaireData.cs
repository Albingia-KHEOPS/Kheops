using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess.Data {
    public class PeriodeEffetAffaireData : AffaireBaseData {
        public DateTime? Debut { get; set; }
        public DateTime? Fin { get; set; }
    }
}
