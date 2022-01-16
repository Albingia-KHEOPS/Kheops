using ALBINGIA.Framework.Common.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesPages
{
    public class ModeleContexteCible
    {
        [Display(Name = "Contexte")]
        public string ContexteCible { get; set; }
        public List<AlbSelectListItem> ContextesCibles { get; set; }
    }
}