using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFinOffre
{
    public class ModeleFinOffreInfos
    {
        [Display(Name = "Antécédents")]
        public List<AlbSelectListItem> Antecedents { get; set; }
        [Display(Name = "Antécédent")]
        public string Antecedent { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Validité de l'offre (j)*")]
        public int ValiditeOffre { get; set; }
        [Display(Name = "Date du projet")]
        public DateTime? DateProjet { get; set; }
        [Display(Name = "Date statistique")]
        public DateTime? DateStatistique { get; set; }
        [Display(Name = "Relance ?")]
        public bool Relance { get; set; }
        [Display(Name = "(j)")]
        public int RelanceValeur { get; set; }
        [Display(Name = "Préavis de résil. (mois)")]
        public int Preavis { get; set; }
        public bool IsReadOnly { get; set; }
        public string Periodicite { get; set; }
        public string DateDebStr { get; set; }

        public ModeleFinOffreInfos()
        {
            Antecedents = new List<AlbSelectListItem>();
            Description = string.Empty;
            ValiditeOffre = 0;
            DateProjet = DateTime.Now;
            DateStatistique = new DateTime();
            Relance = false;
            RelanceValeur = 0;
            Preavis = 0;
        }
    }
}