using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleRefExprCompPage : MetaModelsBase
    {
        public List<AlbSelectListItem> ListeLCI { get; set; }
        public List<AlbSelectListItem> ListeFranchise { get; set; }
    }
}