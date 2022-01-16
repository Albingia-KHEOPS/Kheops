using ALBINGIA.Framework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public enum RegularisationMode
    {
        [EnumMember]
        [BusinessCode("")]
        Unknow = 0,

        [EnumMember]
        [BusinessCode("STAND")]
        Standard = 1,

        [EnumMember]
        [BusinessCode("COASS")]
        Coassurance,

        [EnumMember]
        [BusinessCode("PB")]
        PB,

        [EnumMember]
        [BusinessCode("BNS")]
        BNS,

        [EnumMember]
        [BusinessCode("BURNER")]
        Burner
    }
}
