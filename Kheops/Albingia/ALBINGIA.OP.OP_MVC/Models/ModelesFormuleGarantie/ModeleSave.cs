using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    public class ModeleSave
    {
        public string GuidId { get; set; }
        public List<ModeleNiv1Save> Modeles { get; set; }
    }
}