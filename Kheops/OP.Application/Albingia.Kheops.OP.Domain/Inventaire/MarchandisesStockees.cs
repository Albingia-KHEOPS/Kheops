using Albingia.Kheops.OP.Domain.Referentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class MarchandisesStockees : Marchandises
    {
        public string Lieu { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public Pays Pays { get; set; }
    }
}