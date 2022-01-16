using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public enum  NatureGeneration
    {
        [BusinessCode("")] None,
        [BusinessCode("O")] Obligatoire,
        [BusinessCode("P")] Proposee,
        [BusinessCode("S")] Suggeree

    }
}
