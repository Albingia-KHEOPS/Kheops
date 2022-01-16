using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    public class BlocSave
    {
        public bool MAJ { get; set; }
        public int GuidId { get; set; }
        public bool isChecked { get; set; }
        public List<ModeleSave> Modeles { get; set; }
    }
}