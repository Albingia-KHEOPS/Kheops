using System.Collections.Generic;

namespace Hexavia.Models
{
    public class AdressesWrapper
    {
        /// <summary>
        /// Adresses
        /// </summary>
        public List<Adresse> Adresses { get; set; }

        /// <summary>
        /// HasCedex
        /// </summary>
        public bool HasCedex { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Overflow
        /// </summary>
        public bool Overflow { get; set; }

        /// <summary>
        /// AucuneVoieNeConvient
        /// </summary>
        public bool AucuneVoieNeConvient { get; set; }
    }
}
