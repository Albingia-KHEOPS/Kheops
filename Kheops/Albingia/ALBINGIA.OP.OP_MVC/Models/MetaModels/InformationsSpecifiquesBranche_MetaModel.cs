using System;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    [Serializable]
    public class InformationsSpecifiquesBranche_MetaModel : MetaModelsBase
    {
        [Display (Name="Bonification")]
        public bool Bonification { get; set; }
        [Display(Name = "anticipée")]
        public bool Anticipee { get; set; }
        public bool hiddenAnticipee { get; set; }
        public string Taux { get; set; }
        public bool IsSimpleFolder { get; set; }
        public bool IsEditSimpleFolder { get; set; }
        public bool IsMesageVersionSimpleFolder { get; set; }
    }
}