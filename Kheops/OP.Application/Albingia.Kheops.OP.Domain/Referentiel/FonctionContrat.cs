using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain {
    [Flags]
    public enum FonctionContrat {
        GestionnaireProduction = 0x01,
        Souscripteur = 0x02,
        GestionnaireSinistre = 0x04
    }
}
