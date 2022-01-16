using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesClauses
{
    public class ModeleContexte
    {
        [Display(Name = "Contexte*")]
        public string Contexte { get; set; }
        public List<AlbSelectListItem> Contextes { get; set; }       
    }
}