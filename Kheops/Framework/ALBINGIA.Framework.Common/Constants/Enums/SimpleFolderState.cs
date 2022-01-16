
using System.Runtime.Serialization;

namespace ALBINGIA.Framework.Common.Constants {

    [DataContract(Name = "SimpleFolderState")]
    public enum SimpleFolderState {
        [EnumMember]
        SFFinalaized = 1
            ,
        [EnumMember]
        SFCurrent = 2
            ,
        [EnumMember]
        NSFNew = 3
            ,
        [EnumMember]
        NSFNewWithVersion = 4
    }

}