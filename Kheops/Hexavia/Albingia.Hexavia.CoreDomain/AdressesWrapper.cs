using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albingia.Hexavia.CoreDomain
{
    public class AdressesWrapper
    {
        public List<Adresse> Adresses { get; set; }

        public bool HasCedex { get; set; }

        public int Count { get; set; }

        public bool Overflow { get; set; }

        public bool AucuneVoieNeConvient { get; set; }
    }
}
