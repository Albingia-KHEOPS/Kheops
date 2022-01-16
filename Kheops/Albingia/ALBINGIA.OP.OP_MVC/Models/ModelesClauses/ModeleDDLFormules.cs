using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleDDLFormules
    {
        [Display(Name = "Formule")]
        public string Formule { get; set; }
        public List<AlbSelectListItem> Formules { get; set; }
        
        public ModeleDDLOptions DDLOptions { get; set; }
    }
}