
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.Common
{
    public enum TypeRelation
    {
        [BusinessCode("A")] Associe,
        [BusinessCode("I")] Incompatible,
        [BusinessCode("D")] Dependant,
    }
}
