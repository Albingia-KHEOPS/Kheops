using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesInventaire
{
    public class ModeleInventaireListeCPVille
    {
        public string Type { get; set; }
        public string Valeur { get; set; }
        public List<AlbSelectListItem> Listes { get; set; }
    }
}