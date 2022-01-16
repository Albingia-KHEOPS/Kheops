using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamFamilles;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamFamillesPage : MetaModelsBase
    {
        public string AdditionalParam { get; set; }
        public string RestrictionParam { get; set; }
        [Display(Name = "Concept")]
        public string CodeConcept { get; set; }
        public string LibelleConcept { get; set; }       
        [Display(Name = "Code")]
        public string CodeFamilleRecherche { get; set; }
        [Display(Name = "Description")]
        public string LibelleFamilleRecherche { get; set; }
        public List<LigneFamille> ListeFamilles { get; set; }
    }
}