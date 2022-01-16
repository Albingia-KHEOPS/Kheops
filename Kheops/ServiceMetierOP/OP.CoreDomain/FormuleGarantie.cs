using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class FormuleGarantie
    {
        public bool FormuleChecked { get; set; }
        public List<Volet> Volets { get; set; }
        public string CodeOption { get; set; }
    }
}
