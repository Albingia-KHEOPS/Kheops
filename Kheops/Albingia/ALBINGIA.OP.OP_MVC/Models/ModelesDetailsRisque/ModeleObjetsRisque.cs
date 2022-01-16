using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsRisque
{
    public class ModeleObjetsRisque
    {
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string CodeRisque { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsModifHorsAvenant { get; set; }
        public string LibRisque { get; set; }

        public bool IsModeAvenant { get; set; }
        public bool IsIndexe { get; set; }

        public DateTime? DateModificationAvenant { get; set; }
        public DateTime? DateMinModificationObjet { get; set; }
        public List<ModeleDetailsObjetRisque> Objets { get; set; }

        public string Periodicite { get; set; }
        public string TypePolice { get; set; }
        public bool IsAvnDisabled { get; set; }
    }
}