using Albingia.Kheops.OP.Domain.Affaire;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe
{
    public class ExpressionComplexeBase
    {
        //public AffaireId AffaireId { get; set; }
        public long DesiId { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }

        public bool IsModifiable { get; set; }

        public OrigineExpression Origine { get; set;  }
        public long Id { get; set; }

        public string Code { get; set; }

        virtual public ICollection<ExpressionComplexeDetailBase> Details { get; set; }

        internal void ResetIds() {
            Id = default;
            foreach (var detail in (Details ?? Enumerable.Empty<ExpressionComplexeDetailBase>())) {
                detail.ResetIds();
            }
        }
    }
}