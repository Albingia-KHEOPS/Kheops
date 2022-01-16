using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.DataAccess.Data {
    public class PeriodeConnexiteData {
        public long CodeEngagement { get; set; }
        public int DateDebut { get; set; }
        public int DateFin { get; set; }
        public string CodeTraite { get; set; }
        public long Montant { get; set; }
        public long MontantTotal { get; set; }
        public string CodeOffre { get; set; }
        public int Version { get; set; }
        public string Type { get; set; }
        public string Active { get; set; }
        public string EnCours { get; set; }
        public int Ordre { get; set; }
    }
}
