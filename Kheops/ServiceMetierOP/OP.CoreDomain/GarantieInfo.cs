using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class GarantieInfo
    {
        public string Sequence { get; set; }
        public string Type { get; set; }
        public string Base { get; set; }
        public string Valeur { get; set; }
        public string Unite { get; set; }
        public string MAJ { get; set; }
    }
}
