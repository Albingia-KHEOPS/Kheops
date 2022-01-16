using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formule
{
    public class NatureSelection
    {
        public bool IsAffiche { get; set; }
        public bool IsModifiable { get; set; }
        public NatureValue Checked { get; set; }
        public NatureValue Unchecked { get; set; }
    }
}
