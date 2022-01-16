using ALBINGIA.OP.OP_MVC.Models.ModelesFormGarantie;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class FormuleGarantiePage
    {
        public string Lettre { get; set; }
        public string Libelle { get; set; }

        public List<ModeleFormVolet> Volets { get; set; }
    }
}