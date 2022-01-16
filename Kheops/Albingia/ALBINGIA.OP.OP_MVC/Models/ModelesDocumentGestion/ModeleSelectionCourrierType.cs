using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDocumentGestion
{
    public class ModeleSelectionCourrierType
    {      
        public List<LigneCourrierType> ListeCourriersType { get; set; }
        public string SelectedValue { get; set; }
        public string Filtre { get; set; }
    }
}