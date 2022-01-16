using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesInventaire
{
    [Serializable]
    public class ModeleInventaireGrid
    {
        public string InventaireType { get; set; }
        public List<ModeleInventaireGridRow> InventaireInfos { get; set; }
    }
}