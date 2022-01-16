using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Extension
{
    public static class ObjectMapExtension
    {
        public static Tout Map<Tin,Tout>(this Tin value, Func<Tin, Tout> map)
        {
            return map(value);
        }

        public static IEnumerable<Tin> Single<Tin>(this Tin value)
        {
            yield return value;
        }
        public static IEnumerable<Tout> MapSingle<Tin, Tout>(this Tin value, Func<Tin, Tout> map)
        {
            yield return map(value);
        }
    }
}
