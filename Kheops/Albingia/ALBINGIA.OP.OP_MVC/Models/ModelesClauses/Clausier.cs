using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class Clausier
    {
        [Display(Name = "Libellé")]
        public string Titre { get; set; }

        [Display(Name = "Mots-Clés")]
        public string MotCle1 { get; set; }
        public List<AlbSelectListItem> MotCles1 { get; set; }
        public string MotCle2 { get; set; }
        public List<AlbSelectListItem> MotCles2 { get; set; }
        public string MotCle3 { get; set; }
        public List<AlbSelectListItem> MotCles3 { get; set; }

        [Display(Name = "Rubrique")]
        public string Rubrique { get; set; }
        public List<AlbSelectListItem> Rubriques { get; set; }
        public ModeleDDLSousRubrique ModeleDDLSousRubrique { get; set; }
        public ModeleDDLSequence ModeleDDLSequence { get; set; }

        public string CodeEtapeAjout { get; set; }
        public string LibelleEtapeAjout { get; set; }
        public string CodeOffre { get; set; }
        public string CodeContexte { get; set; }
        public string LibelleContexte { get; set; }
        public int NbrObjetsRisque1 { get; set; }
        public string CodeRisqueObjet { get; set; }
        public string RisqueObjet{get;set;}
        public int NbrRisques { get; set; }
        public ModeleObjetsRisque ObjetsRisqueAll { get; set; }

        public bool SearchScreen { get; set; }
       
    }
}