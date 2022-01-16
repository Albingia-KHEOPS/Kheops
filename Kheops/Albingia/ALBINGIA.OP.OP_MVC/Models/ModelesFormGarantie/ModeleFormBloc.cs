using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormGarantie
{
    public class ModeleFormBloc
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ModeleFormModele> Modeles { get; set; }
    }
}