using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesGarantieType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    [Serializable]
    public class ModeleGarantieTypePage : MetaModelsBase
    {
        public ModeleRechercheGarantieType RechercheGarantieType { get; set; }
        public List<ModeleGarantieType> GarantieTypes { get; set; }
        public ModeleGarantieType GarantieType { get; set; }
        public bool IsModifiable { get; set; }

        public ModeleGarantieTypePage()
        {
            this.RechercheGarantieType = new ModeleRechercheGarantieType();
            this.GarantieTypes = new List<ModeleGarantieType>();
            this.GarantieType = new ModeleGarantieType();
        }
    }
}