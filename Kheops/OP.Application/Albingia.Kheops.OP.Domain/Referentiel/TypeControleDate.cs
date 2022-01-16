using Albingia.Kheops.Common;
using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public enum TypeControleDateEnum
    {
        [BusinessCode("")] Strict,
        [BusinessCode("NDEB")] IgnoreStart,
        [BusinessCode("NFIN")] IgnoreEnd,
        [BusinessCode("NONCL")] Ignore,

    }

    public class TypeControleDate: RefValue { 
    }
}
