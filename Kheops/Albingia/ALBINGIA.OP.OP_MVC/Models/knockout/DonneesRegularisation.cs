using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ALBINGIA.OP
{
    [DataContract]
    public class DonneesRegularisation
    {
        public decimal CotisationPeriode { get; set; }

        public string PrcCotisationPeriode { get; set; }

        public string CotisationsRetenues { get; set; }

        public string PrcCotisationsRetenues { get; set; }

        public decimal ChargementSinistres { get; set; }

        public decimal MontantRistourneAnticipee { get; set; }

        public string RistourneAnticipee { get; set; }

        public string LibelleMontant { get; set; }

        public string MontantCalcule { get; set; }

        public string MontantAffiche { get; set; }

        public string RistourneMontant { get; set; }
    }
}