using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleDDLSousRubrique
    {
        [Display(Name = "Sous-rubrique")]
        public string SousRubrique { get; set; }
        public List<AlbSelectListItem> SousRubriques { get; set; }

        public ModeleDDLSequence ModeleDDLSequence { get; set; }

        public ModeleDDLSousRubrique()
        {
            SousRubrique = string.Empty;
            SousRubriques = new List<AlbSelectListItem>();
            ModeleDDLSequence = new ModeleDDLSequence();
        }
    }
}