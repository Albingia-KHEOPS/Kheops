
using ALBINGIA.Framework.Common;
using System.Runtime.Serialization;

namespace OP.WSAS400.DTO.Regularisation
{
    [DataContract]
    public enum RegularisationScope
    {
        [EnumMember]
        [BusinessCode("")]
        Unknow = 0,

        [EnumMember]
        [BusinessCode("E")]
        Contrat = 1,

        [EnumMember]
        [BusinessCode("R")]
        Risque,

        [EnumMember]
        [BusinessCode("M")]
        Garantie
    }
}
