using ALBINGIA.OP.OP_MVC.Models.ModelesRisque;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleCreationRegularisation
{
    public class ModeleRisquesRegularisation
    {
        public Int32 ReguleId { get; set; }
        public string TypeAvt { get; set; }
        public Int32 CodeAvn { get; set; }

        public DateTime? DateDebRsq { get; set; }
        public DateTime? DateFinRsq { get; set; }
        public List<ModeleRisque> Risques { get; set; }
    }
}