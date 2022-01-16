using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Mvc.Common {
    public enum AccesStatutVerrou {
        NonVerrouille = 0,
        Verrouille,
        Proprietaire,
        VerrouilleProprietaire
    }
}