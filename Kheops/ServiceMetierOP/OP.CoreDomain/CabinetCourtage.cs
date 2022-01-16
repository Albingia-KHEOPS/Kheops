using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class CabinetCourtage
    {
        public int Code { get; set; }
        public string NomCabinet { get; set; }
        public List<string> NomSecondaires { get; set; }
        public string Type { get; set; }
        public Adresse Adresse { get; set; }
        public List<Interlocuteur> Interlocuteurs { get; set; }
        public bool ValideInterlocuteur { get; set; }
        public bool EstValide { get; set; }
        public DateTime? FinValidite { get; set; }
        public Delegation Delegation { get; set; }

        public CabinetCourtage()
        {
            NomSecondaires = new List<string>();
        }

    }
}
