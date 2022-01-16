using Albingia.Kheops.OP.Domain.Referentiel;
using System;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    public class ParamExpressionComplexeDetailFranchise : ParamExpressionComplexeDetailBase
    {
        public decimal ValeurMin { get; set; }
        public decimal ValeurMax { get; set; }
        public Indice CodeIncide { get; set; }
        public decimal IncideValeur { get; set; }

        public DateTime? LimiteDebut { get; set; }
        public DateTime? LimiteFin { get; set; }


    }
}