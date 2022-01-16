using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Parametre
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string Descriptif { get; set; }
        public bool Utilise { get; set; }
    }
}
