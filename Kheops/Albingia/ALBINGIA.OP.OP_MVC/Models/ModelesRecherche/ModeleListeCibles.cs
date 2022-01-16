using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModelesRecherche
{
    public class ModeleListeCibles
    {
        [Display(Name = "Cible")]
        public String Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }
    }
}