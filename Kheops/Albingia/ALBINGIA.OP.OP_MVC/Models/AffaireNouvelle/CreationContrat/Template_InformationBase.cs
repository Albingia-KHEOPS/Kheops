using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.AffaireNouvelle.CreationContrat
{
    public class Template_InformationBase
    {
         [Display(Name = "Canevas")]
        public String Template { get; set; }
        public List<AlbSelectListItem> Templates { get; set; }

        public bool CopyMode { get; set; }
        public bool IsReadOnly { get; set; }
        public bool TemplateMode { get; set; }

        /// <summary>
        /// Permet de savoir si la page est chargée dynamiquement sur changement de template dans la liste
        /// </summary>
        public bool LoadTemplateMode { get; set; }
              
    }
}