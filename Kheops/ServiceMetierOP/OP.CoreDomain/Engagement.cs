using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class Engagement
    {
        public bool LCIGenerale { get; set; }
        public string LCIValeur { get; set; }
        public string LCIUnite { get; set; }
        public List<Parametre> LCIUnites { get; set; }
        public string LCIType { get; set; }
        public List<Parametre> LCITypes { get; set; }
        public bool LCIIndexee { get; set; }

        public string Nature { get; set; }
        public string PartAlb { get; set; }
        public string Couverture { get; set; }

        public List<EngagementTraite> Traites { get; set; }

        public string BaseTotale { get; set; }
        public string BaseAlb { get; set; }
    }
}
