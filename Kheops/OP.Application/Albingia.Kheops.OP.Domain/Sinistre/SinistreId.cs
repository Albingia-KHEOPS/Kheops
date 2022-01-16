using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class SinistreId {
        public DateTime DateSurvenance { get; set; }
        public int Numero { get; set; }
        public Branche SousBranche { get; set; }
    }
}
