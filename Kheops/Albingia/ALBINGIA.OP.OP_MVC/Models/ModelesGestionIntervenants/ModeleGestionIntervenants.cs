using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesGestionIntervenants
{
    public class ModeleGestionIntervenants
    {
        public string CodeDossier { get; set; }
        public string VersionDossier { get; set; }
        public string TypeDossier { get; set; }
        public List<ModeleIntervenant> ListIntervenants { get; set; }

        public bool IsModeAvenant { get; set; }
        public bool IsAvenantModificationLocale { get; set; }
        public bool IsReadOnly { get; set; }
    }
}