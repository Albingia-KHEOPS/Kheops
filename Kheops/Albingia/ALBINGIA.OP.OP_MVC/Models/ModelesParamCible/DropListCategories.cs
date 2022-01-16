using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamCible
{
    public class DropListCategories
    {
        public string Id { get; set; }
        public List<AlbSelectListItem> Categories { get; set; }
        public string SelectedValue { get; set; }
    }
}