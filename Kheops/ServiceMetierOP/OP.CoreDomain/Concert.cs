using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.CoreDomain
{
    ////[Serializable]
    public class Concert
    {
        public string CodeInventaire { get; set; }
        public string NumeroLigne { get; set; }
        public string Descriptif { get; set; }
        public string NomSite { get; set; }
        public string NatureLieu { get; set; }
        public Adresse Adresse { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public Decimal? Budget { get; set; }
        public int? NbEvent { get; set; }
    }
}
