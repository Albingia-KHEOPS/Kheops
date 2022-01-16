using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class Delegation
    {
        public string Code { get; set; }
        public string Nom { get; set; }
        public Inspecteur Inspecteur { get; set; }
        public string Secteur { get; set; }
    }
}
