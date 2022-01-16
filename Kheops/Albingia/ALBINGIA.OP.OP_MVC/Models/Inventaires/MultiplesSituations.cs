using ALBINGIA.Framework.Common.Extensions;
using System;

namespace ALBINGIA.OP.OP_MVC.Models.Inventaires
{
    public class MultiplesSituations
    {
        public string RaisonSociale { get; set; }

        public string Adresse { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public LabeledValue Qualité { get; set; }

        public LabeledValue Renonciation { get; set; }

        public decimal Surface { get; set; }

        public double Contenu { get; set; }

        public LabeledValue RisquesLocatifs { get; set; }

        public double ValeurRisquesLocatifs { get; set; }

        public double ValeurMaterielsBureautique { get; set; }
    }
}