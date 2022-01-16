using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.ModelePage
{
    [Serializable]
    public class AnInformationsSpecifiquesBranchePage : MetaModelsBase
    {    
        [Display(Name = "Bonification")]
        public bool Bonification { get; set; }
        [Display(Name = "anticipée")]
        public bool Anticipee { get; set; }
        public bool hiddenAnticipee { get; set; }
        public string Taux { get; set; }        
    }
}