using ALBINGIA.Framework.Common;
using System.Runtime.Serialization;

namespace ALBINGIA.Framework.Business {
    public enum TypeConnexite {
        [BusinessCode("01")]
        Remplacement = 1,
        [BusinessCode("10")]
        Information = 10,
        [BusinessCode("20")]
        Engagement = 20,
        [BusinessCode("30")]
        Resiliation = 30,
        [BusinessCode("40")]
        Regularisation = 40
    }
}
