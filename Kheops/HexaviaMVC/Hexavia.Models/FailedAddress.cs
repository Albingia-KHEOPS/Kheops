using System;

namespace Hexavia.Models
{
    [Serializable]
    public class FailedAddress
    {
        public string Address { get; set; }
        public int? NumeroChrono { get; set; }
    }
}
