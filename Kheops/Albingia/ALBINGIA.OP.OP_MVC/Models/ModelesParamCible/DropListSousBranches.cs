using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class DropListSousBranches
    {
        public string Id { get; set; }
        public List<AlbSelectListItem> SousBranches { get; set; }
        public string SelectedValue { get; set; }
    }
}