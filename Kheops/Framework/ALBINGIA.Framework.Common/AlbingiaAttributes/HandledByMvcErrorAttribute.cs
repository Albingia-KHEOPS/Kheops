using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common {

    [AttributeUsage(validOn: AttributeTargets.Property,Inherited =true)]
    public class HandledByMvcErrorAttribute: Attribute {

    }
}
