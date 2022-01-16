using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class ManifestationAssure : ActiviteLocalisee
    {
        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public decimal Montant { get; set; }
    }
}