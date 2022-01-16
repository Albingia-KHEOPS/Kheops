using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class Cible_InformationBase
    {
        [Display(Name = "Cible *")]
        public String Cible { get; set; }
        public List<AlbSelectListItem> Cibles { get; set; }
        public bool EditMode { get; set; }
        public bool TemplateMode { get; set; }
        public bool CopyMode { get; set; }
        public bool IsReadOnly { get; set; }
        public bool LoadTemplateMode { get; set; }
    }
}