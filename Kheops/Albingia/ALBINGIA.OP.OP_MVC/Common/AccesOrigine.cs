using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Mvc.Common {
    public enum AccesOrigine {
        Consulter = 0,
        CreationAffaire,
        CreationAvenant,
        EtablirAffaireNouvelle,
        Modifier,
        ModifierHorsAvenant,
        RetourPiece,
        PrisePosition,
        Avenant,
        RepriseAvenant,
        Resiliation,
        RemiseEnVigueur,
        Regularisation,
        RegularisationEtAvenant,
        Engagements,
        Attestation,
        BlocageDeblocageTermes
    }
}