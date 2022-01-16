using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.Regularisation.ModeleSaisieRegulGarantie
{
    public class SaisieInfoRegulPeriod
    {
        public InfoInitSaisieRegul InfoInitregul { get; set; }
        public List<AlbSelectListItem> UnitesDef { get; set; }
        public List<AlbSelectListItem> CodesTaxesDef { get; set; }
        public List<AlbSelectListItem> UnitesPrev { get; set; }
        public List<AlbSelectListItem> CodesTaxesPrev { get; set; }
        public bool IsSimplifiedRegul { get; set; }
    }
}