using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleChoixClauseTransverse : MetaModelsBase
    {
        // Filtres
        [Display(Name = "Etape")]
        public string Etape { get; set; }
        public List<AlbSelectListItem> Etapes { get; set; }
        [Display(Name = "Contexte")]
        public string Contexte { get; set; }
        public List<AlbSelectListItem> Contextes { get; set; }
        [Display(Name = "Risque")]
        public string Risque { get; set; }
        public List<AlbSelectListItem> Risques { get; set; }
        public ModeleDDLFormules DDLFormules { get; set; }      
        [Display(Name = "Contexte")]
        public string ContexteCible { get; set; }
        public List<AlbSelectListItem> ContextesCibles { get; set; }
        [Display(Name = "Filtrer")]
        public string Filtre { get; set; }
        public List<AlbSelectListItem> Filtres { get; set; }
        // Tableau
        public List<ModeleClause> TableauClauses { get; set; }
        public bool FullScreen { get; set; }
    }
}