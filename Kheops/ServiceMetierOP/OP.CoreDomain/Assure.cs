using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class Assure
    {
        public string Code { get; set; }
        public string NomAssure { get; set; }
        public List<string> NomSecondaires { get; set; }
        public Adresse Adresse { get; set; }
        public int Siren { get; set; }

        public Assure()
        {
            NomSecondaires = new List<string>();
        }
    }
}
