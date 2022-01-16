using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    public class DetailsGarantie
    {
        public string Code { get; set; }
        public string Libelle { get; set; }

        public Risque Risque { get; set; }

        public bool LciIndexe { get; set; }
        public bool FranchiseIndexe { get; set; }
        public bool AssietteIndexe { get; set; }
        public bool CATNAT { get; set; }

        public string Nature { get; set; }
        public List<Parametre> Natures { get; set; }

        public DetailsGarantie()
        {
            Code = String.Empty;
            Libelle = String.Empty;
            Risque = new Risque();
            LciIndexe = false;
            FranchiseIndexe = false;
            AssietteIndexe = false;
            CATNAT = false;
            Nature = String.Empty;
            Natures = new List<Parametre>();
        }
    }
}
