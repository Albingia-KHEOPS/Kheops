using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Interlocuteur
    {
        public string Id { get; set; }
        public string IdSecondaire { get; set; }
        public string Nom { get; set; }
        public bool EstValide { get; set; }
        public CabinetCourtage CabinetCourtage { get; set; }
    }
}
