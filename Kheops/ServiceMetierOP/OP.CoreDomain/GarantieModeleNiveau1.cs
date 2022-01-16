using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class GarantieModeleNiveau1
    {
        public int Code { get; set; }
        public string CodeGarantie { get; set; }
        public string Description { get; set; }
        public string Caractere { get; set; }
        public string Nature { get; set; }
        public string NatureParam { get; set; }
        public int CodeParent { get; set; }
        public int CodeNiv1 { get; set; }
        public List<GarantieModeleNiveau2> Modeles { get; set; }
        public bool AppliqueA { get; set; }

        public string GuidGarantie { get; set; }
    }
}
