using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesInformationsSpecifiques
{
    [Serializable]
    public class ModeleListOptions
    {
        [Display(Name = "Option")]
        public String Option { get; set; }
        public List<AlbSelectListItem> Options { get; set; }
    }
}