using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Souscripteur
    {
        public string Code { get; set; }
        public string Login { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public Branche Branche { get; set; }
        public Delegation Delegation { get; set; }
    }
}
