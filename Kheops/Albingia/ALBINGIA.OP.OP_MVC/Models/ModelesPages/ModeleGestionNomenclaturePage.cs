using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesGestionNomenclature;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleGestionNomenclaturePage : MetaModelsBase
    {
        [Display(Name="Typologie")]
        public string Typologie { get; set; }
        public List<AlbSelectListItem> Typologies { get; set; }
        [Display(Name="Branche")]
        public string Branche { get; set; }
        public List<AlbSelectListItem> Branches { get; set; }
        [Display(Name="Cible")]
        public string Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }

        public List<ModeleNomenclature> Nomenclatures { get; set; }

        public ModeleNomenclature AddNomenclature { get; set; }
    }
}