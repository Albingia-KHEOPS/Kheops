using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //[Serializable]
    public class Modele
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreation { get; set; }
        public string Caractere { get; set; }
        public string GuidId { get; set; }

        public List<GarantieModeleNiveau1> Modeles { get; set; }
    }
}
