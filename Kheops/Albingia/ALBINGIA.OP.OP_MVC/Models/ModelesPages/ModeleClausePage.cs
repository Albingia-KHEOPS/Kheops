using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleLibellesClauses;
using System;
using System.Collections.Generic;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    
    public class ModeleClausePage : MetaModelsBase
    {

       // public List<AlbSelectListItem> Branches { get; set; }
       // public List<AlbSelectListItem> Cibles { get; set; }
        public ModeleRechercheClauses RechercheClauses { get; set; }
        public List<ModeleClause> Clauses { get; set; }
        public ModeleClause Clause { get; set; }

        public ModeleClausePage()
        {
            this.RechercheClauses = new ModeleRechercheClauses();
            this.Clauses = new List<ModeleClause>();
            this.Clause = new ModeleClause();
        }
    }
}