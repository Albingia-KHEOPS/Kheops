using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesParamIS;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleParamISPage : MetaModelsBase
    {
        public ParamModeleIntermediaire Intermediaire { get; set; }
        // Listes de bases
        public List<AlbSelectListItem> Branches { get; set; }
        public List<AlbSelectListItem> Sections { get; set; }

    }
}