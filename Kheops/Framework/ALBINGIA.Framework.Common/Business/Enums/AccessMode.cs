using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Business {
    [DataContract]
    public enum AccessMode {
        [EnumMember]
        CONSULT,

        [EnumMember]
        CREATE,

        [EnumMember]
        UPDATE
    }
}
