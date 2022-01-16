using System;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    [Serializable]
    public class Garantie_Index_MetaModel : MetaModelsBase
    {
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }

        public Garantie_Index_MetaModel()
        {
            this.CodeFormule = string.Empty;
            this.CodeOption = string.Empty;
        }
    }
}