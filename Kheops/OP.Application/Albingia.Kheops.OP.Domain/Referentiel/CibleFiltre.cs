using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class CibleFiltre
    {
        public CibleFiltre()
        {

        }
        public CibleFiltre(string branche, string cible)
        {
            this.CodeBranche = branche;
            this.CodeCible = cible;
        }
        public string CodeBranche {get; set;}
        public string CodeCible { get; set;}
    }
}
