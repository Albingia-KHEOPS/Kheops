
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Affaire {
    public enum TraitementAffaire {
        [BusinessCode("")] Inconnu = 0,
        [BusinessCode("OFFRE")] Offre,
        [BusinessCode("AFFNV")] AffaireNouvelle,
        [BusinessCode("AVNMD")] Avenant,
        [BusinessCode("REGUL")] Regularisation,
        [BusinessCode("AVNRG")] RegularisationEtAvenant,
        [BusinessCode("PB")] PB,
        [BusinessCode("AVNRS")] Resiliation,
        [BusinessCode("AVNRM")] RemiseEnVigueur,
        [BusinessCode("ATTES")] Attestation
    }
}