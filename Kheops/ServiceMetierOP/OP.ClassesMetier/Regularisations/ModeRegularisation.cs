using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.ClassesMetier
{
    [DataContract]
    public enum ModeRegularisation
    {
        Standard = 0,
        Assiette,
        Coassurance,
        PB,
        BNS,
        Burner
    }
}
