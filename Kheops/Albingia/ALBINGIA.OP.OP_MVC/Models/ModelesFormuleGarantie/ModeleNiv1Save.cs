using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesFormuleGarantie
{
    public class ModeleNiv1Save
    {
        public bool MAJ { get; set; }
        public string GuidGarantie { get; set; }
        public string Caractere { get; set; }
        public string Nature { get; set; }
        public string NatureParam { get; set; }
        public List<ModeleNiv2Save> Modeles { get; set; }
        public string Action { get; set; }

        public string ParamNatMod { get; set; }
    }
}