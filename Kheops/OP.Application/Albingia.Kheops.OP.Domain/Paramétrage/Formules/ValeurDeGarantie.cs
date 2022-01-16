using Albingia.Kheops.OP.Domain.Formule;
using Albingia.Kheops.OP.Domain.Referentiel;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    public class ValeurDeGarantie
    {
        public TypeDeValeur Type { get; set; }
        public bool Modifiable { get; set; }
        public bool Obligatoire { get; set; }
        public AlimentationValue TypeAlimentation {  get;set;}
        public Unite Unite { get; set; } = new Unite();
        public BaseDeCalcul CoseBase { get; set; } = new BaseDeCalcul();
        public virtual ParamExpressionComplexeBase Expression { get; set; }
        public decimal Valeur { get; set; }
    }
}