
using ALBINGIA.Framework.Common;

namespace ALBINGIA.Framework.Common.Constants {
    /// <summary>
    /// Types de droit utilisateur
    /// </summary>
    public enum TypeDroit {
        [AlbEnumInfoValue("M_Master")]
        M,
        [AlbEnumInfoValue("A_Administration")]
        A,
        [AlbEnumInfoValue("G_Gestion")]
        G,
        [AlbEnumInfoValue("V_Visualisation")]
        V
    }
}