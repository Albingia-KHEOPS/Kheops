using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class Branche
    {
        public string Code { get; set; }
        public string Nom { get; set; }
        public Cible Cible { get; set; }
    }
}
