
namespace ALBINGIA.Framework.Common.Constants
{
    /// <summary>
    ///  LablatResult
    ///  Vérification Lablat sur une entité morale ou physique et renvoie une réponse à Kheops. 
    ///  Les types de réponses sont : 
    ///  0: Entité non suspecte 
    ///  1: Entité suspecte 
    /// -1: Entité Black Listée
    /// </summary>
    public enum LablatResult
    {

        Suspicious,

        Notsuspicious,

        Blacklisted,

        Error
    }
}