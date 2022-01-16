using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Risque
{
    public class ObjetId
    {
        public AffaireId AffaireId { get; set; }

        public int NumRisque { get; set; }

        public int NumObjet { get; set; }
    }
}
