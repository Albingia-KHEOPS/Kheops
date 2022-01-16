using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    //Serializable]
    public class EnsembleGarantie
    {
        public string Id { get; set; }
        public string IdSequence { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Couleur1 { get; set; }
        public string Couleur2 { get; set; }
        public List<LigneGarantie> LstLigneGarantie { get; set; }
        
        public string Niveau { get; set; }
        public string Pere { get; set; }
        public string Sequence { get; set; }
        public string Origine { get; set; }

        public string CodeBloc { get; set; }

        public bool ReadOnly { get; set; }

        public EnsembleGarantie()
        {
            LstLigneGarantie = new List<LigneGarantie>();
        }
    }
}
