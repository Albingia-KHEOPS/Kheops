using Albingia.Kheops.OP.Domain.Referentiel;
using System;

namespace Albingia.Kheops.OP.Domain.Inventaire
{
    public class MultipleSituation : InventaireItem
    {
        public string RaisonSociale { get; set; }

        public string Adresse { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public QualiteJuridique Qualite { get; set; }

        public Renonciation Renonciation { get; set; }

        public decimal Surface { get; set; }

        public decimal Contenu { get; set; }

        public RisqueLocatif RisquesLocatifs { get; set; }

        public decimal ValeurRisquesLocatifs { get; set; }

        public decimal ValeurMaterielsBureautique { get; set; }
    }
}