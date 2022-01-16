using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Parametrage
{
    public class Filtre
    {
        public string Code { get; set; }
        public long Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<LigneFiltre> Lines { get; set; }

        public bool DoesApply(CibleFiltre cible)
        {
            var hasExclusion = Lines.Any(x => x.IsExclusion);
            var hasInclusion = Lines.Any(x => x.IsInclusion);
            if (!hasExclusion && !hasInclusion) return true;

            if (hasInclusion)
            {
                return Lines.OrderBy(x => x.Ordre).Aggregate(false, (a, x) =>
                     a || (x.DoesApply(cible) && (x.IsInclusion || !x.IsExclusion)));
            }
            else
            {
                return Lines.OrderBy(x => x.Ordre).Aggregate(true, (a, x) =>
                        a && (!x.IsExclusion || !(x.DoesApply(cible))));
            }
        }
    }
}
