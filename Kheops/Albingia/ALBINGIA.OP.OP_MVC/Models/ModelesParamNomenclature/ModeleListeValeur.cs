using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamNomenclature
{
    public class ModeleListeValeur
    {
        public List<AlbSelectListItem> ListeValeurs { get; set; }
        public string SelectedValue { get; set; }
        public string IdColonne { get; set; }
        public string GuidIdLigne { get; set; }
    }
}