using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Regularisation {
    public enum NiveauRegularisation {
        [BusinessCode("")]
        Inconnu = 0,
        
        [BusinessCode("E")]
        Contrat = 1,
        
        [BusinessCode("R")]
        Risque,
        
        [BusinessCode("M")]
        Garantie
    }
}
