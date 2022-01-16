using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleDDLSequence
    {
        [Display(Name = "Séquence")]
        public string Sequence { get; set; }
        public List<AlbSelectListItem> Sequences { get; set; }

        public ModeleDDLSequence()
        {
            Sequence = string.Empty;
            Sequences = new List<AlbSelectListItem>();
        }
    }
}