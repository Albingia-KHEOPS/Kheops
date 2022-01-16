using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albingia.Hexavia.CoreDomain
{
    [Serializable]
    public class Pays
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }
}
