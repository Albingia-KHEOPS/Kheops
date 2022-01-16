using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess.Data {
    public class KDA300Result {
        public int AnneeDebut { get; set; }
        public int MoisDebut { get; set; }
        public int JourDebut { get; set; }
        public int AnneeFin { get; set; }
        public int MoisFin { get; set; }
        public int JourFin { get; set; }
        public string CodeErreur { get; set; }
        public long CodeCourtierPayeur { get; set; }
        public long CodeCourtierCommission { get; set; }
        public string CodeEncaissement { get; set; }
        public int DernierAvn { get; set; }
        public bool MultiCourtier { get; set; }
    }
}
