using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class DropListBranches
    {
        public List<AlbSelectListItem> Branches { get; set; }
        public string SelectedValue { get; set; }
    }
}