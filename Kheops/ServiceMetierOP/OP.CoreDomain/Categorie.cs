using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    ////[Serializable]
    public class Categorie
    {
        public string GuidId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreation { get; set; }
        public string CodeBranche { get; set; }
        public string Caractere { get; set; }
    }
}
