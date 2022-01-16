using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO
{
    [DataContract]
    public enum AvenantAccess {
        [EnumMember]
        CREATE,

        [EnumMember]
        UPDATE,

        [EnumMember]
        CONSULT
    }
}
