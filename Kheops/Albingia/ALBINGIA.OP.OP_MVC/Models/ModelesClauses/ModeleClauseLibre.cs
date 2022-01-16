using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleClauseLibre
    {
        public List<AlbSelectListItem> ContexteList { get; set; }
        public string Contexte { get; set; }

        public string Risque { get; set; }
        public ModeleObjetsRisque ObjetsRisqueAll { get; set; }
        public int NbrObjets { get; set; }
        public string CodeRsq { get; set; }
        public string DescRsq { get; set; }
        public bool IsRsqSelected { get; set; }
    }
}