using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesOffresVerrouillees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleOffresVerrouilleesPage : MetaModelsBase
    {
        public List<ModeleOffreVerrouillee> OffresVerrouillees;

        [Display(Name = "Date de début")]
        public DateTime? DateDebutFiltre;
        [Display(Name = "Date de fin")]
        public DateTime? DateFinFiltre;
        [Display(Name = "Utilisateur")]
        public string TypeOffreFiltre;
        public string NumeroOffreFiltre;       
        public string VersionFiltre;
        public string UtilisateurFiltre;
        public Int64 NbOffreBdd { get; set; }
        public Int64 NbOffreCache { get; set; }
    }
}