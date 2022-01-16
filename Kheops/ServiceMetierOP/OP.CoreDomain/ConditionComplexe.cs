using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
   // //[Serializable]
    public class ConditionComplexe
    {
        public string Type { get; set; }
        public string Libelle { get; set; }
        public string Code { get; set; }
        public string Id { get; set; }
        public string Descriptif { get; set; }
        public List<Parametre> Expressions { get; set; }
        public List<LigneGarantie> LstLigneGarantie { get; set; }
        
        public string UniteNew { get; set; }
        public string TypeNew { get; set; }
        public string UniteConcurrence { get; set; }
        public string TypeConcurrence { get; set; }
        
        public ConditionComplexe()
        {
            Expressions = new List<Parametre>();
            LstLigneGarantie = new List<LigneGarantie>();
        }
    }
}
