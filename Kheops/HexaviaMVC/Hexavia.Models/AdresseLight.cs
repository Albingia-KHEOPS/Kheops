using System;

namespace Hexavia.Models
{
    [Serializable]
    public class AdresseLight
    {
        public int? NumeroChrono { get; set; }
        public string AdresseComplete4 { get; set; }    
        public string AdresseCompleteSansCP4 { get; set; }
        public string AdresseComplete3 { get; set; }
        public string AdresseCompleteSansCP3 { get; set; }
        public string AdresseComplete5 { get; set; }
        public string AdresseCompleteSansCP5 { get; set; }
        public string AdresseComplete { get; set; }
        public string AdresseCompleteSansCP { get; set; }       
    }
}
