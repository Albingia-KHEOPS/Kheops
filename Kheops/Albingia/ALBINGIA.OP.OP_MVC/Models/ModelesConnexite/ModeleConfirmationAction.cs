using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesConnexite
{
    public class ModeleConfirmationAction
    {
        public string TypeConnexite { get; set; }
        public string codeTypeConnexite { get; set; }

        [Display(Name = "Contrat actuel: ")]
        public string NumOffreActuelle { get; set; }
        public string VersionOffreActuelle { get; set; }
        public string TypeOffreActuelle { get; set; }
        public string BrancheOffreActuelle { get; set; }
        public string SousBrancheOffreActuelle { get; set; }
        public string CatOffreActuelle { get; set; }

        public string NumConnexiteActuelle { get; set; }
        public long CodeObservationConnexiteActuelle { get; set; }
        public string ObservationConnexiteActuelle { get; set; }     
        public string IdeConnexiteActuelle { get; set; }
        public List<ModeleLigneConnexite> GroupeConnexiteActuel { get; set; }

        [Display(Name = "Contrat à ajouter: ")]
        public string NumOffreOrigine { get; set; }
        public string VersionOffreOrigine { get; set; }
        public string TypeOffreOrigine { get; set; }
        public string BrancheOffreOrigine { get; set; }
        public string SousBrancheOffreOrigine { get; set; }
        public string CatOffreOrigine { get; set; }
        public string IdeConnexiteOrigine { get; set; }
        public string NumConnexiteOrigine { get; set; }
        public long CodeObservationConnexiteOrigine { get; set; }
        public string ObservationConnexiteOrigine { get; set; }       
        public List<ModeleLigneConnexite> GroupeConnexiteOrigine { get; set; }
    }
}