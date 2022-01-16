using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class ManifestationAssure : ActiviteLocalisee
    {
        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public double Montant { get; set; }
    }
}