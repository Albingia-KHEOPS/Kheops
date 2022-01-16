using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamNomenclature
{
    public class ModeleListeFamille
    {
        public List<AlbSelectListItem> ListeFamilles { get; set; }
        public string SelectedValue { get; set; }
        public string Id { get; set; }
    }
}