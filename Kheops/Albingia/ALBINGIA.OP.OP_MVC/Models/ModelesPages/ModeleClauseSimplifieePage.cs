using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModeleMatriceFormule;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleClauseSimplifieePage : MetaModelsBase
    {
        [Display(Name = "Etape")]
        public string Etape { get; set; }
        public List<AlbSelectListItem> Etapes { get; set; }

        [Display(Name = "Filtre")]
        public string Filtre { get; set; }
        public List<AlbSelectListItem> Filtres { get; set; }

        [Display(Name = "Contexte")]
        public string Contexte { get; set; }
        public List<AlbSelectListItem> Contextes { get; set; }

        //[Display(Name = "Contexte")]
        //public string ContexteCible { get; set; }
        //public List<AlbSelectListItem> ContextesCibles { get; set; }

        public ModeleContexteCible ModeleContexteCible { get; set; }


        public List<ModeleClause> Clauses { get; set; }

        public bool FullScreen { get; set; }

        public string AppliqueA { get; set; }
        public List<ModeleClauseSimpRisque> Risques { get; set; }
        public List<ModeleMatriceFormuleForm> Formules { get; set; }
        public int NbrObjetsRisque1 { get; set; }
        public int NbrRisques { get; set; }
        public string CodeRisqueObjet { get; set; }       
        public bool IsRsqSelected { get; set; }
    }
}