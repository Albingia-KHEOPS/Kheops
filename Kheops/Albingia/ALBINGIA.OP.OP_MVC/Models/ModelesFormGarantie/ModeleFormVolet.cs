using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormGarantie
{
    public class ModeleFormVolet
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ModeleFormBloc> Blocs { get; set; }
    }
}