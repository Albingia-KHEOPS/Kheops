using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class PreneurAssurance
    {
        [Display(Name = "Code *")]
        public String Numero { get; set; }

        [Display(Name = "Nom")]
        public String Nom { get; set; }

        [Display(Name = "Dépt")]
        public String Departement { get; set; }

        [Display(Name = "Ville")]
        public String Ville { get; set; }

        [Display(Name = "Le preneur d'assurance est un assuré")]
        public bool PreneurEstAssure { get; set; }

        public int NbAssuAdditionnel { get; set; }

        //public void SetPreneurAssurance(ContratInfoBaseDto contratInfoBaseDto)
        //{
        //    if (contratInfoBaseDto.CodePreneurAssurance != 0)
        //        this.Numero = contratInfoBaseDto.CodePreneurAssurance.ToString();
        //    this.Nom = contratInfoBaseDto.NomPreneurAssurance;
        //    this.Departement = contratInfoBaseDto.DepAssurance;
        //    this.Ville = contratInfoBaseDto.VilleAssurance;
        //}
    }
}