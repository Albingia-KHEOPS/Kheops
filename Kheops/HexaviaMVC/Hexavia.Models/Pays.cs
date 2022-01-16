using System;

namespace Hexavia.Models
{
    /// <summary>
    /// Pays
    /// </summary>
    [Serializable]
    public class Pays
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Libelle
        /// </summary>
        public string Libelle { get; set; }
    }
}
