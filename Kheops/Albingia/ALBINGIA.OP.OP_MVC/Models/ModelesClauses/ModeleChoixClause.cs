using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleChoixClause : MetaModelsBase
    {
        // Filtres
        [Display(Name = "Etape")]
        public string Etape { get; set; }
        public List<AlbSelectListItem> Etapes { get; set; }
        [Display(Name = "Contexte")]
        public string Contexte { get; set; }
        public List<AlbSelectListItem> Contextes { get; set; }
        [Display(Name = "Contexte")]
        public string ContexteCible { get; set; }
        public List<AlbSelectListItem> ContextesCibles { get; set; }
        public List<AlbSelectListItem> ContextesCiblesCode { get; set; }
        [Display(Name = "Filtrer")]
        public string Filtre { get; set; }
        public List<AlbSelectListItem> Filtres { get; set; }
        public List<ModeleClause> TableauClauses { get; set; }
        public bool FullScreen { get; set; }
    }
}