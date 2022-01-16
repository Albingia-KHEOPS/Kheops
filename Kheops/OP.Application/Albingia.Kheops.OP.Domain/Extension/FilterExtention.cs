using Albingia.Kheops.OP.Domain.Parametrage;
using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Application.Infrastructure.Extension
{
    public static class FilterExtention
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> filterable, Branche branche, Cible cible) where T:IFiltered {
            return filterable.Filter(new CibleFiltre(branche.Code, cible.Code));
        }
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> filterable, string branche, string cible) where T : IFiltered
        {
            return filterable.Filter(new CibleFiltre(branche, cible));
        }
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> filterable, CibleFiltre cible) where T : IFiltered
        {
            if (cible == null) {
                cible = new CibleFiltre(String.Empty, String.Empty);
            }
            return filterable.Where(x => x.Filtre?.DoesApply(cible) ?? true);
        }
    }
}
