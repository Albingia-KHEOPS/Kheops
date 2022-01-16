using ALBINGIA.Framework.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ALBINGIA.OP.OP_MVC.Models.ModeleCreationSaisie
{
    public class ModeleInfoSaisieTemplate
    {
        [Display(Name = "Canevas")]

        public String Template { get; set; }
        public List<AlbSelectListItem> Templates { get; set; }

        public bool CopyMode { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsReadOnlyDisplay { get; set; }

        /// <summary>
        /// Permet de savoir si la page est chargée dynamiquement sur changement de template dans la liste
        /// </summary>
        public bool LoadTemplateMode { get; set; }

        public ModeleInfoSaisieTemplate()
        {
            Templates = new List<AlbSelectListItem>();
        }

    }
}