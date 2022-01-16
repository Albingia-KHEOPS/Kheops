using System.Collections.Generic;

namespace Albingia.Kheops.OP.Domain.Parametrage.Formules
{
    public class ParamExpressionComplexeBase
    {
        public long DesiId { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }

        public long Id { get; set; }

        public string Code { get; set; }
        public bool IsModifiable { get; set; }


        virtual public ICollection<ParamExpressionComplexeDetailBase> Details { get; set; }
    }
}