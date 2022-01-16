using ALBINGIA.Framework.Common.Extensions;
using ALBINGIA.OP.OP_MVC.Models.MetaModels;
using ALBINGIA.OP.OP_MVC.Models.ModelesClauses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleClausierPage : MetaModelsBase
    {
        [Display(Name = "Offre")]
        public string CodeOffre { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string Etape { get; set; }
        public string Branche { get; set; }
        //public string Contexte { get; set; }
        //public int DateCreationOffre { get; set; }

        [Display(Name = "Risque / Objet :")]
        public string TypeInventaire { get; set; }

        [Display(Name = "Libellé")]
        public string Titre { get; set; }

        [Display(Name = "Mots-Clés")]
        public string MotCle1 { get; set; }
        public List<AlbSelectListItem> MotCles1 { get; set; }
        public string MotCle2 { get; set; }
        public List<AlbSelectListItem> MotCles2 { get; set; }
        public string MotCle3 { get; set; }
        public List<AlbSelectListItem> MotCles3 { get; set; }

        public string Formule { get; set; }
        public string Risque { get; set; }
        public string CodeRsq { get; set; }
        public string CodeFormule { get; set; }
        public string CodeOption { get; set; }

        [Display(Name = "Rubrique")]
        public string Rubrique { get; set; }
        public List<AlbSelectListItem> Rubriques { get; set; }
        public ModeleDDLSousRubrique ModeleDDLSousRubrique { get; set; }
        public ModeleDDLSequence ModeleDDLSequence { get; set; }
        [Display(Name = "Mode")]
        public string Mode { get; set; }
        public List<AlbSelectListItem> Modes { get; set; }
        public ModeleContexte ModeleContexte { get; set; }

        [Display(Name = "s'applique à l'objet ou le risque")]
        public string ObjetRisque { get; set; }
        public string ObjetRisqueCode { get; set; }
        public ModeleObjetsRisque ObjetsRisqueAll { get; set; }
        public int NbrObjets { get; set; }
        public bool IsRsqSelected { get; set; }

        public bool PleinEcran { get; set; }
    }
}