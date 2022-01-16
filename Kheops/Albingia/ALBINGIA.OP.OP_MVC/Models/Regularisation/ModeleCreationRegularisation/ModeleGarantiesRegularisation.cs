using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation
{
    public class ModeleGarantiesRegularisation
    {
        public string TypeAvt { get; set; }
        public Int32 CodeAvn { get; set; }

        public DateTime? DateDebGar { get; set; }
        public DateTime? DateFinGar { get; set; }

        public ModeleRisque Risque { get; set; }
        public List<ModeleGarantie> Garanties { get; set; }
    }
}