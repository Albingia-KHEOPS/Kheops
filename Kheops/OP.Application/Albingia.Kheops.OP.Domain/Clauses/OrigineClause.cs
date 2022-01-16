using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALBINGIA.Framework.Common;

namespace Albingia.Kheops.OP.Domain.Clauses
{
    public enum OrigineClause
    {
        [BusinessCode("")] None,
        [BusinessCode("GAR")] Garantie,
        [BusinessCode("ASS")] Assure,
        [BusinessCode("OPP")] Opposition,
        [BusinessCode("DLI")] DetailLigneInventaire,
        [BusinessCode("INV")] Inventaire,
        [BusinessCode("COA")] Coassurance,
        [BusinessCode("TDI")] TableauDetailLigneInventaire
    }
}
