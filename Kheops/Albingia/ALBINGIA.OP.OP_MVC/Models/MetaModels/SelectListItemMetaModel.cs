using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.MetaModels
{
    public class SelectListItemMetaModel
    {
        public string Item { get; set; }
        //public IEnumerable<AlbSelectListItem> Items { get; set; }
        public IList<AlbSelectListItem> Items { get; set; }
    }
}