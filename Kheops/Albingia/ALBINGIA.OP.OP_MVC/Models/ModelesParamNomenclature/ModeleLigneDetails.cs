using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesParamNomenclature
{
    public class ModeleLigneDetails
    {
        public int GuidId { get; set; }
        public string Valeur01 { get; set; }
        public string Valeur02 { get; set; }
        public string Valeur03 { get; set; }
        public string Valeur04 { get; set; }

        public List<AlbSelectListItem> ListeValeurs01 { get; set; }
        public List<AlbSelectListItem> ListeValeurs02 { get; set; }
        public List<AlbSelectListItem> ListeValeurs03 { get; set; }
        public List<AlbSelectListItem> ListeValeurs04 { get; set; }

    }
}