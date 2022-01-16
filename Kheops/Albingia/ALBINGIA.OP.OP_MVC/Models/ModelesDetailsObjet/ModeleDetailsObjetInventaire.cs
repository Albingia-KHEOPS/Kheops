using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesDetailsObjet
{
    public class ModeleDetailsObjetInventaire
    {
        public string CodeInventaire { get; set; }
        [Display(Name = "Type inventaire")]
        public string TypeInventaire { get; set; }
        public List<AlbSelectListItem> TypesInventaire { get; set; }
        public ModeleInventaireObjet Inventaires { get; set; }
        public string TypeList { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsModifHorsAvenant { get; set; }
        public bool? IsAvnDisabled { get; set; }
    }
}