using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    public class PrimeId : AffaireId {
        virtual public int Numero { get; set; }
        virtual public int NumeroEcheance { get; set; }
    }
}
