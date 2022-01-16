using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesEcheance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleEcheancierPage : MetaModelsBase
    {
        [Display(Name = "Prime HT (€)")]
        public double PrimeHT { get; set; }
        [Display(Name = "Frais accessoires HT (€)")]
        public double FraisAccessoiresHT { get; set; }
        [Display(Name = "Période du")]
        public String PeriodeDebut { get; set; }
        [Display(Name = "au")]
        public String PeriodeFin { get; set; }

        [Display(Name = "ou Comptant HT* (€)")]
        public double ComptantHT { get; set; }
        [Display(Name = "% prime")]
        public double PrimeComptant { get; set; }
        public decimal PrimeComptantCalcule { get; set; }
        [Display(Name = "Frais accessoires HT (€)")]
        public int FraisAccessoiresComptantHT { get; set; }
        [Display(Name = "FGA")]
        public bool TaxeAttentat { get; set; }
        public List<ModeleEcheance> Echeances { get; set; }
        public double MontantRestant { get; set; }

        public bool IsModeSaisieParMontant { get; set; }
       
    }
}