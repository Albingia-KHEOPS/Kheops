using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public class Unite : RefValue {
        public static implicit operator string(Unite unit) {
            return unit?.Code ?? string.Empty;
        }
    }
    public class UniteCapitaux : Unite
    {
    }
    public class UniteLCI : Unite
    {
    }
    public class UniteFranchise : Unite
    {
    }
    public class UnitePrime : Unite
    {
    }

    public class UniteValeurRisque : Unite
    {
    }
}
