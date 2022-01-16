using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class AdresseRisque
    {
        [Display(Name = "Bâtiment | ZI")]
        public string Batiment { get; set; }
      
        [Display(Name = "N° | Ext | Voie")]
        public string No { get; set; }
  
        [Display(Name = "Ext.")]
        public string Extension { get; set; }
      
        [Display(Name = "voie")]
        public string Voie { get; set; }
 
        [Display(Name = "Distribution")]
        public string Distribution { get; set; }
    
        [Display(Name = "CP | Ville")]
        public string CodePostal { get; set; }
      
        [Display(Name = "Ville")]
        public string Ville { get; set; }
      
        [Display(Name = "CP | Ville (Cedex)")]
        public string CodePostalCedex { get; set; }
      
        [Display(Name = "Ville")]
        public string VilleCedex { get; set; }
     
        [Display(Name = "Pays")]
        public string Pays { get; set; }

        public bool ReadOnly { get; set; }
    }
}