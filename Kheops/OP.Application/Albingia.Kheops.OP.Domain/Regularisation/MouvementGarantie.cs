using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public class MouvementGarantie {
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public decimal Assiette { get; set; }
        public UniteBase UniteTauxBase { get; set; }
        public decimal MontantBase { get; set; }
        public decimal TauxBase { get; set; }
        public long IdGarantie { get; set; }
        public string CodeGarantie { get; set; }
    }
}
