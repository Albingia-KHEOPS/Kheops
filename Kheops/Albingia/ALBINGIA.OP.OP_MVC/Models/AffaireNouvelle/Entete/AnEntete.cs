using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.Entete
{
    public class AnEntete
    {
        [Display(Name = "Contrat :")]
        public string IdContrat { get; set; }
        public string Cible { get; set; }
        public string Branche { get; set; }
        [Display(Name = "Délégation :")]
        public string Delegation { get; set; }
        [Display(Name = "Gestionnaire :")]
        public string GestionnaireNom { get; set; }
        public string GestionnaireCode { get; set; }
        public string DateDebutEffet { get; set; }
        public string DateFinEffet { get; set; }
        [Display(Name = "Périodicité :")]
        public string Periodicite { get; set; }
    }
}