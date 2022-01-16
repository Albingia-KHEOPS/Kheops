using Albingia.Kheops.OP.Domain.Parametrage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albingia.Kheops.OP.Domain.Referentiel
{
    public abstract class RefParamValue : RefValue
    {
        public Decimal ParamNum1 { get; set; }
        public Decimal ParamNum2 { get; set; }
        public string ParamText1 { get; set; }
        public string ParamText2 { get; set; }

      
    }
}
