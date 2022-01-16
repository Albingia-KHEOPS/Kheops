using Albingia.Kheops.OP.Domain.Formules.ExpressionComplexe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Affaire
{
    public class Expressions
    {
        public Expressions()
        {
            Franchises = new Dictionary<string, ExpressionComplexeFranchise>();
            LCIs = new Dictionary<string, ExpressionComplexeLCI>(); 
        }
        public Expressions(IEnumerable<ExpressionComplexeBase> expressions)
        {
            // Manage rare cases of duplicates in database, Distinct via groupBy.
            this.Franchises = expressions.OfType<ExpressionComplexeFranchise>().GroupBy(x => x.Code).ToDictionary(x=>x.Key, x=>x.First());
            this.LCIs = expressions.OfType<ExpressionComplexeLCI>().GroupBy(x => x.Code).ToDictionary(x => x.Key, x => x.First());
        }
        public Dictionary<string, ExpressionComplexeFranchise> Franchises { get; private set; }
        public Dictionary<string, ExpressionComplexeLCI> LCIs { get; private set; }
        public IEnumerable<ExpressionComplexeBase> All => this.Franchises.Values.Cast<ExpressionComplexeBase>().Concat(this.LCIs.Values);
    }
}
