using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleDDLOptions
    {
        [Display(Name = "Option")]
        public string Option { get; set; }
        public List<AlbSelectListItem> Options { get; set; }
    }
}