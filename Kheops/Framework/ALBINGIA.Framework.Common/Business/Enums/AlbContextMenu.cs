
using System.ComponentModel.DataAnnotations;

namespace ALBINGIA.Framework.Common.Constants {
    public enum AlbContextMenu {
        [Display(Name = "None")]
        NONE,
        [Display(Name = "Nouveau")]
        CREER,
        [Display(Name = "Offre/Contrat")]
        OFFCONT,
        [Display(Name = "Validation")]
        VALIDATION,
        [Display(Name = "Régularisation")]
        REGULE,
        [Display(Name = "Actes de gestion")]
        AVENANT,
        [Display(Name = "Reprise")]
        REPRISE,
        [Display(Name = "Prendre Position")]
        PRENDPOS,
        [Display(Name = "Modifier")]
        OPMODIFIER,
        [Display(Name = "Consulter")]
        CONSULTER,
        [Display(Name = "Avenant de Résiliation")]
        AVNRS,
        [Display(Name = "Avenant de Remise en vigueur")]
        AVNRM
    }
}